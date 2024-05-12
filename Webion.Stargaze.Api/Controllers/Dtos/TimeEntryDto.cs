namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TimeEntryDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid? TaskId { get; init; }
    
    public required string? Description { get; init; }
    
    public required DateTimeOffset Start { get; init; }
    public required DateTimeOffset End { get; init; }
    public required TimeSpan Duration { get; init; }

    public required bool Locked { get; init; }
    public required bool Billable { get; init; }
    public required bool Billed { get; init; }
    public required bool Paid { get; init; }
    
    
}