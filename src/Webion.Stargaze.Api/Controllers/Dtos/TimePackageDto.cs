namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TimePackageDto
{
    public required Guid Id { get; init; }
    public required Guid CompanyId { get; init; }
    public required string? Name { get; init; }
    public required int TotalHours { get; init; }
    public required double RemainingHours { get; set; }
    public required double TrackedHours { get; set; }
    
    public required decimal Earnings { get; set; }
    public required decimal Cost { get; set; }
    
    public required IEnumerable<Guid> AppliesTo { get; init; }
    public required IEnumerable<Guid> Users { get; init; }
}