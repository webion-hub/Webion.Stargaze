using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class Priority4Dto
{
    public required string Priority { get; init; }
    public required string Color { get; init; }

    [JsonPropertyName("orderindex")]
    public required string OrderIndex { get; init; }
    public required string Id { get; init; }
}