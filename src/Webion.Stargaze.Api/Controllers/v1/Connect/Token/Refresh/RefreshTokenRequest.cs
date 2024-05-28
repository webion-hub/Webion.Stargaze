using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Refresh;

public sealed class RefreshTokenRequest
{
    [Required] public Guid UserId { get; init; }
    [Required] public Guid ClientId { get; init; }
    [Required] public string ClientSecret { get; init; } = null!;
    [Required] public string RefreshToken { get; init; } = null!;
}