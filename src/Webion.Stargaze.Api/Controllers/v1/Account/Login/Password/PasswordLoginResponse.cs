namespace Webion.Stargaze.Api.Controllers.v1.Account.Login.Password;

public sealed class PasswordLoginResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}