using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Login.External;

public sealed class ExternalLoginRequest
{
    [Required]
    public Guid ClientId { get; init; }
    
    [Required, Url]
    public string ExchangeUri { get; init; } = null!;
    
    [Required, Url]
    public string RedirectUri { get; init; } = null!;
}