namespace Webion.ClickUp.Api.V2.Common;

public sealed class TaskLocationDto
{
    public string? ListId { get; init; }
    public string? FolderId { get; init; }
    public string? SpaceId { get; init; }
    
    public string? ListName { get; init; }
    public string? FolderName { get; init; }
    public string? SpaceName { get; init; }
}