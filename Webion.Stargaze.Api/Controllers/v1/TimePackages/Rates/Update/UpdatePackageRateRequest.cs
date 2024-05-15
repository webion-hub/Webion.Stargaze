using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Update;

public sealed class UpdatePackageRateRequest
{
    [Required]
    public decimal Rate { get; init; }
}