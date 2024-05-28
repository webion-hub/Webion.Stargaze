using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class Status11Dto
{
    public string? Id { get; init; }
    public required string Status { get; init; }
    public required string Color { get; init; }

    [JsonPropertyName("orderindex")]
    public int OrderIndex { get; init; }
    public string? Type { get; init; }
}