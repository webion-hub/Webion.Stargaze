using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Create;

public sealed class CreateTaskRequest
{
    [Required]
    public Guid ProjectId { get; init; }

    [Required]
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public TimeSpan? TimeEstimate { get; init; }
}