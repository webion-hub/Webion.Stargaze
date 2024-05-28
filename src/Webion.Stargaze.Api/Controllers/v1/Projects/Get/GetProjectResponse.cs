namespace Webion.Stargaze.Api.Controllers.v1.Projects.Get;

public sealed class GetProjectResponse
{
    public required Guid Id { get; init; }
    public required Guid CompanyId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
}