namespace Webion.Stargaze.Auth.Services.Jwt;

public sealed record TokenPair(
    string AccessToken,
    string RefreshToken
);