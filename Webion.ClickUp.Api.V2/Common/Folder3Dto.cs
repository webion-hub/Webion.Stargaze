namespace Webion.ClickUp.Api.V2.Common;

public sealed class Folder3Dto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required bool Hidden { get; init; }
    public required bool Access { get; init; }
}