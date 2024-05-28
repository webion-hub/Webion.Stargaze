using System.Text.Json.Serialization;
using Webion.ClickUp.Api.V2.Converters;

namespace Webion.ClickUp.Api.V2.Common;

public sealed class Datum2Dto
{
    public required string Id { get; init; } = null!;
    public Task4Dto? Task { get; init; }
    public required string Wid { get; init; }
    public required User2Dto User { get; init; } = null!;
    public bool Billable { get; init; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public required DateTimeOffset Start { get; init; }
    public string? Description { get; init; }
    public string[] Tags { get; init; } = [];
    public string? At { get; init; }
}