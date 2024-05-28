namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TimePackageDto
{
    public required Guid Id { get; init; }
    public required int TotalHours { get; init; }
    public required string? Name { get; init; }
    public double RemainingHours { get; set; }
    public double TrackedHours { get; set; }
}