namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TimePackageRateDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid PackageId { get; init; }
    public required decimal Rate { get; init; }
}