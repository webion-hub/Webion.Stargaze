using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.Update;

public sealed class UpdateTimeEntryRequest
{
    [Required]
    public Guid UserId { get; init; }

    [Required]
    public DateTimeOffset Start { get; init; }

    [Required]
    public DateTimeOffset End { get; init; }

    [Required]
    public bool Billable { get; init; }

    public Guid? TaskId { get; init; }
    public string? Description { get; init; }
}