using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.Common;

public sealed class User2Dto
{
    public required ulong Id { get; init; }
    public required string Username { get; init; }
    public required string Initials { get; init; }
    public required string Email { get; init; }
    public required string Color { get; init; }
    
    [JsonPropertyName("profilePicture")]
    public required string ProfilePicture { get; init; }
}