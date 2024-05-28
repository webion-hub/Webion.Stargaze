using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace Webion.AspNetCore.Authentication.ClickUp;

public sealed class ClickUpHandler : OAuthHandler<ClickUpOptions>
{
    public ClickUpHandler(IOptionsMonitor<ClickUpOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        if (tokens.AccessToken is null)
            throw new InvalidOperationException("No access token was provided");

        Backchannel.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokens.AccessToken);
        var userInfo = await Backchannel.GetFromJsonAsync<GetUserResponse>(Options.UserInformationEndpoint);
        if (userInfo is null)
            throw new InvalidOperationException("Error while deserializing user info");

        identity.AddClaims(
            GetClaims(userInfo, tokens)
        );

        return await base.CreateTicketAsync(identity, properties, tokens);
    }

    private static IEnumerable<Claim> GetClaims(GetUserResponse response, OAuthTokenResponse tokens)
    {
        yield return new Claim(ClaimTypes.NameIdentifier, response.User.Id.ToString());
        yield return new Claim(ClaimTypes.Name, response.User.UserName);

        if (tokens.AccessToken is not null)
            yield return new Claim(ClickUpClaims.AccessToken, tokens.AccessToken);

        if (response.User.ProfilePicture is not null)
            yield return new Claim(ClickUpClaims.ProfilePicture, response.User.ProfilePicture);

        if (response.User.Color is not null)
            yield return new Claim(ClickUpClaims.Color, response.User.Color);
    }
}

internal sealed class GetUserResponse
{
    public UserDto User { get; set; } = null!;

    internal sealed class UserDto
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }

        [JsonPropertyName("username")]
        public string UserName { get; init; } = null!;

        [JsonPropertyName("color")]
        public string? Color { get; init; }

        [JsonPropertyName("profilePicture")]
        public string? ProfilePicture { get; init; }
    }
}
