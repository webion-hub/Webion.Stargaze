namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TrackedTimeDto
{
    public required Guid ProjectId { get; init; }
    public required double TotalHours { get; set; }
}