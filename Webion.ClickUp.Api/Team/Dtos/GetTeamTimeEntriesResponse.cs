using System.Text.Json.Serialization;
using Webion.ClickUp.Api.Common;
using Webion.ClickUp.Api.Converters;

namespace Webion.ClickUp.Api.Team.Dtos;

public sealed class GetTeamTimeEntriesResponse
{
    public IEnumerable<Datum1> Data { get; init; } = [];
    
    public sealed class Datum1
    {
        public required string Id { get; init; } = null!;
        public Task4Dto? Task { get; init; } = null!;
        public required string Wid { get; init; } = null!;
        public required User2Dto User { get; init; } = null!;
        public required bool Billable { get; init; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public required DateTimeOffset Start { get; init; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public required DateTimeOffset End { get; init; }
        
        [JsonConverter(typeof(MsDurationConverter))]
        public required TimeSpan Duration { get; init; }
        
        public required string Description { get; init; }
        public required string[] Tags { get; init; } = [];
        public required string Source { get; init; }
        public required string At { get; init; }
        public required TaskLocationDto TaskLocation { get; init; }
        public required string TaskUrl { get; init; }
    }
}

