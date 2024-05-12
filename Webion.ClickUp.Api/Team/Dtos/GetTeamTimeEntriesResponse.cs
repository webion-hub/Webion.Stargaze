using System.Text.Json;
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
        
        /// <summary>
        /// A property of type <see cref="Task4Dto"/>.
        /// </summary>
        /// <remarks>
        /// If not set it may be <c>null</c> or a <see cref="string"/>.
        /// </remarks>
        public JsonElement? Task { get; init; }
        
        public required string Wid { get; init; }
        public required User2Dto User { get; init; } = null!;
        public bool Billable { get; init; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public required DateTimeOffset Start { get; init; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public required DateTimeOffset End { get; init; }
        
        [JsonConverter(typeof(MsDurationConverter))]
        public required TimeSpan Duration { get; init; }
        
        public string? Description { get; init; }
        public string[] Tags { get; init; } = [];
        public string? Source { get; init; }
        public string? At { get; init; }
        
        public TaskLocationDto? TaskLocation { get; init; }
        public string? TaskUrl { get; init; }
    }
}

