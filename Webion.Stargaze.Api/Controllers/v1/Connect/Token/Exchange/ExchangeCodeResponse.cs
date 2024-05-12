namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Exchange;

public sealed class ExchangeCodeResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}