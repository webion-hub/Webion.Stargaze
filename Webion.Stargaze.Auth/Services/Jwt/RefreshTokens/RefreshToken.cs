namespace Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;

public record struct RefreshToken(Guid Id, byte[] Secret)
{
    public const int SecretSize = 64;
}