namespace Webion.ClickUp.Api.Common;

public sealed class Task4Dto
{
    public required string Id { get; init; }
    public string? CustomId { get; init; }
    public required string Name { get; init; }
    public required StatusDto Status { get; init; }
    public required string? CustomType { get; init; }
}