using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class StatusDto
{
    public required string Status { get; init; }
    public required string Color { get; init; }
    
    [JsonPropertyName("orderindex")]
    public required int OrderIndex { get; init; }
    public required string Type { get; init; }
}