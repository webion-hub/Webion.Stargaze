using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Add;

public sealed class CreatePackageRateRequest
{
    [Required]
    public Guid UserId { get; init; }
    
    [Required]
    public decimal Rate { get; init; }
}