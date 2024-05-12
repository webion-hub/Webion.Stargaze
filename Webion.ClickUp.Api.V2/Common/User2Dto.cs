using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Common;

[DebuggerDisplay("{Email}")]
public sealed class User2Dto
{
    public required ClickUpId Id { get; init; }
    
    public required string Username { get; init; }
    public required string Initials { get; init; }
    public required string Email { get; init; }
    public required string Color { get; init; }
    
    [JsonPropertyName("profilePicture")]
    public required string ProfilePicture { get; init; }
}