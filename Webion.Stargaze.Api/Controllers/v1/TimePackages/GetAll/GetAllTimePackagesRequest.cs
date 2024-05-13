using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.GetAll;

public sealed class GetAllTimePackagesRequest : PaginatedRequest
{
    public Guid? CompanyId { get; init; }
}