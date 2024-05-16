namespace Webion.ClickUp.Api.V2.Common;

public sealed class Space2Dto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public bool? Access { get; init; }
}