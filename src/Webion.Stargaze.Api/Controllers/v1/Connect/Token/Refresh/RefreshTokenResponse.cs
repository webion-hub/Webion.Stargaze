namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Refresh;

public sealed class RefreshTokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}