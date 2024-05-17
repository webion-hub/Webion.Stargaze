using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class Folders5Dto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    
    [JsonPropertyName("orderindex")]
    public required int OrderIndex { get; init; }
    public required bool OverrideStatuses { get; init; }
    public required bool Hidden { get; init; }
    public required Space2Dto Space { get; init; }
    public required string TaskCount { get; init; }
    public required bool Archived { get; init; }
    public required IEnumerable<StatusDto> Statuses { get; init; }
    public required IEnumerable<List4Dto> Lists { get; init; }
    public required string PermissionLevel { get; init; }
}