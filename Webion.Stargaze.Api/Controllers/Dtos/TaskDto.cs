namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TaskDto
{
    public required Guid Id { get; init; }
    public required Guid ProjectId { get; init; }
    public required string Title { get; init; }
    public required string? Description { get; init; }
}