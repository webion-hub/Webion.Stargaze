namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class ProjectDto
{
    public required Guid Id { get; init; }
    public required Guid CompanyId { get; init; }

    public required string Name { get; init; }
    public required string? Description { get; init; }
}