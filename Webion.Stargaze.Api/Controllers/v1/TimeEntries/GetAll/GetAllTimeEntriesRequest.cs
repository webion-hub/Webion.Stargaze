using System.ComponentModel.DataAnnotations;
using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.GetAll;

public sealed class GetAllTimeEntriesRequest : PaginatedRequest
{
    [Required]
    public DateTime From { get; init; }
    
    [Required]
    public DateTime To { get; init; }
    
    /// <summary>
    /// Filter the time entries by task.
    /// </summary>
    public Guid? TaskId { get; init; }
    
    /// <summary>
    /// Filter the time entries by project.
    /// </summary>
    public Guid? ProjectId { get; init; }

    /// <summary>
    /// Filter the time entries by assignee.
    /// </summary>
    public IEnumerable<Guid> TrackedBy { get; init; } = [];
}