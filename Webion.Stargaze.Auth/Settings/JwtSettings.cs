using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Auth.Settings;


public sealed class JwtSettings
{
    public const string Section = "Jwt";

    [Required] public required TimeSpan AccessTokenDuration { get; init; }
    [Required] public required TimeSpan RefreshTokenDuration { get; init; }
    [Required] public required string Issuer { get; init; }
    [Required] public required string Secret { get; init; }

    public string[] Audiences { get; init; } = [];
}