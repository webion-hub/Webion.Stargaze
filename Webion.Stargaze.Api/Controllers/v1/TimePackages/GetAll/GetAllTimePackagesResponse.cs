using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.GetAll;

public sealed class GetAllTimePackagesResponse
{
    public required IEnumerable<TimePackageDto> TimePackages { get; init; }
    public required double TotalTime { get; init; }
    public required double RemainingBillableTime { get; init; }
}