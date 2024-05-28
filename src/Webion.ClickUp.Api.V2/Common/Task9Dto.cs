using System.Text.Json.Serialization;
using Webion.ClickUp.Api.V2.Converters;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class Task9Dto
{
    public required string Id { get; init; }
    public required string CustomId { get; init; }
    public required int CustomItemId { get; init; }
    public required string Name { get; init; }
    public required string TextContent { get; init; }
    public required string Description { get; init; }
    public required Status11Dto Status { get; init; }
    public string? OrderIndex { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset DateCreated { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset DateUpdated { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset DateClosed { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset DateDone { get; init; }
    public required CreatorDto Creator { get; init; }
    public required IEnumerable<User2Dto> Assignees { get; init; }
    public required IEnumerable<User2Dto> Watchers { get; init; }
    public required IEnumerable<ChecklistDto> Checklists { get; init; }
    public required IEnumerable<TagDto> Tags { get; init; }
    public required Task9Dto? Parent { get; init; }
    public required Priority4Dto? Priority { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset? DueDate { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset? StartDate { get; init; }
    public object? Point { get; init; }
    public required int? TimeEstimate { get; init; }
    public int? TimeSpent { get; init; }
    public required IEnumerable<object> CustomFields { get; init; }
    public required IEnumerable<object> Dependencies { get; init; }
    public required IEnumerable<object> LinkedTasks { get; init; }
    public required IEnumerable<object> Locations { get; init; }
    public required string TeamId { get; init; }
    public required string Url { get; init; }
    public required SharingDto Sharing { get; init; }
    public required string PermissionLevel { get; init; }
    public required ListDto List { get; init; }
    public required ProjectDto Project { get; init; }
    public required FolderDto Folder { get; init; }
    public required SpaceDto Space { get; init; }
}