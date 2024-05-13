using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Update;

public sealed class UpdateTaskRequest
{
    [Required]
    public Guid ProjectId { get; init; }

    [Required]
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
}