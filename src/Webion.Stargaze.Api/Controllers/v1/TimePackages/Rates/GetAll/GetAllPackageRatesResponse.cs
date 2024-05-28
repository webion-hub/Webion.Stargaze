using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.GetAll;

public sealed class GetAllPackageRatesResponse
{
    public required List<TimePackageRateDto> TimePackageRates { get; init; }
}