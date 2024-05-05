using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.Common;

public sealed class UserDto
{
    public required long Id { get; init; }
    
    [JsonPropertyName("username")]
    public required string UserName { get; init; }
    public required string Color { get; init; }
    
    [JsonPropertyName("profilePicture")]
    public required string ProfilePicture { get; init; }
}