using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class ChecklistDto
{
    public required string Id { get; init; }
    public required string TaskId { get; init; }
    public required string Name { get; init; }
    public required string DateCreated { get; init; }

    [JsonPropertyName("orderindex")]
    public required int OrderIndex { get; init; }
    public required int Creator { get; init; }
    public required int Resolved { get; init; }
    public required int Unresolved { get; init; }
    public required IEnumerable<string> Items { get; init; }
}