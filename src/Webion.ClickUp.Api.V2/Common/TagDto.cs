namespace Webion.ClickUp.Api.V2.Common;

public sealed class TagDto
{
    public required string Name { get; init; }
    public required string TagFg { get; init; }
    public required string TagBg { get; init; }
    public required int? Creator { get; init; }
}