using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Exchange;

public sealed class ExchangeCodeRequest
{
    [Required]
    public Guid ClientId { get; init; }
    
    [Required]
    [Base64String]
    public string ClientSecret { get; init; } = null!;
}