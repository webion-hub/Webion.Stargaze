namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Get;

public sealed class GetTaskResponse
{
    public required Guid Id { get; init; }
    public required Guid ProjectId { get; init; }
    public required string Title { get; init; }
    public required string? Description { get; init; }
    public required TimeSpan? TimeEstimate { get; init; }
}