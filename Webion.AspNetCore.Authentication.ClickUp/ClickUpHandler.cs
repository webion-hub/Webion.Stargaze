using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api;
using Webion.ClickUp.Api.V2;

namespace Webion.AspNetCore.Authentication.ClickUp;

public sealed class ClickUpHandler : OAuthHandler<ClickUpOptions>
{
    private readonly IClickUpApi _clickUpApi;
    
    public ClickUpHandler(IOptionsMonitor<ClickUpOptions> options, ILoggerFactory logger, UrlEncoder encoder, IClickUpApi clickUpApi) : base(options, logger, encoder)
    {
        _clickUpApi = clickUpApi;
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        if (tokens.AccessToken is null)
            throw new InvalidOperationException();
        
        var userInfo = await _clickUpApi.OAuth.GetUserAsync(tokens.AccessToken);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.User.Id),
            new Claim(ClaimTypes.Name, userInfo.User.UserName),
            new Claim(ClickUpClaims.AccessToken, tokens.AccessToken),
        };
        
        identity.AddClaims(claims);
        
        return await base.CreateTicketAsync(identity, properties, tokens);
    }
}