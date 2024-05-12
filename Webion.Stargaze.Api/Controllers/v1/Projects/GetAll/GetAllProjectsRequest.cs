using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.GetAll;

public sealed class GetAllProjectsRequest : PaginatedRequest
{
    /// <summary>
    /// Filters projects by company.
    /// </summary>
    public Guid? CompanyId { get; init; }
}