namespace Webion.ClickUp.Api.V2.Common;

public sealed class SpaceDto
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public bool? Hidden { get; init; }
    public bool? Access { get; init; }
}