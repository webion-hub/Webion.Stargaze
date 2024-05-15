namespace Webion.ClickUp.Api.V2.Common.Dtos;

public sealed class Task9
{
    public required string Id { get; init; }
    public required int? CustomItemId { get; init; }
    public required string Name { get; init; }
    public required object Status { get; init; }
    public string? MarkdownDescription { get; init; }
    public required string OrderIndex { get; init; }
    public required DateTimeOffset DateCreated { get; init; }
    public required DateTimeOffset DateUpdated { get; init; }
    public required DateTimeOffset DateClosed { get; init; }
    public required DateTimeOffset DateDone { get; init; }
    public required CreatorDto Creator { get; init; }
    public required IEnumerable<string> Assignees { get; init; }
    public required IEnumerable<string> Watchers { get; init; }
    public required IEnumerable<string> Checklists { get; init; }
    public required IEnumerable<string> Tags { get; init; }
    public required string? Parent { get; init; }
    public required string? Priority { get; init; }
    public required string? DueDate { get; init; }
    public required string? StartDate { get; init; }
    public required string? TimeEstimate { get; init; }
    public required string? TimeSpent { get; init; }
    public required ListDto List { get; init; }
    public required FolderDto Folder { get; init; }
    public required SpaceDto Space { get; init; }
    public required string Url { get; init; }
}
