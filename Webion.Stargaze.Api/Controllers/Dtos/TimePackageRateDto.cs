namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class TimePackageRateDto
{
    public required Guid Id { get; init; }
    public required decimal Rate { get; init; }
}