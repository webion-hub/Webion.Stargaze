namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class ClickUpCurrentTimeEntryDto
{
    public required string? Description { get; init; }
    public required bool Billable { get; init; }

    public required DateTimeOffset Start { get; init; }
    public required TimeSpan Duration { get; init; }

    public Guid? TaskId { get; init; }
    public string? TaskName { get; init; }
    public string? TaskDescription { get; init; }

    public required string? UserUsername { get; init; }
    public required string? UserEmail { get; init; }
}