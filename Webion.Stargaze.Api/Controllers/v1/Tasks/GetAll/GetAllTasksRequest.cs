using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.GetAll;

public sealed class GetAllTasksRequest : PaginatedRequest
{
    public Guid? CompanyId { get; init; }
    public Guid? ProjectId { get; init; }
    
    /// <summary>
    /// Filter the tasks by assignee.
    /// </summary>
    public IEnumerable<Guid> AssignedTo { get; init; } = [];
}