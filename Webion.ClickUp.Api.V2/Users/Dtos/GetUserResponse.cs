using System.Text.Json.Serialization;
using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Users.Dtos;

public sealed class GetUserResponse
{
    public required Member7 Member { get; init; }
}


public sealed class Member7
{
    public required User21 User { get; init; }
    public required InvitedBy InvitedBy { get; init; }
    public required Shared Shared { get; init; }
}

public sealed class User21
{
    public required ClickUpId Id { get; init; }
    
    [JsonPropertyName("username")]
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string? Color { get; init; }
    public required string? ProfilePicture { get; init; }
    public required string Initials { get; init; }

    /// <summary>
    /// Owner = 1, Admin = 2, Member = 3, Guest = 4
    /// </summary>
    public required int Role { get; init; }
    
    public required CustomRole CustomRole { get; init; }
    
    public required DateTime? LastActive { get; init; }
    public required DateTime? DateJoined { get; init; }
    public required DateTime? DateInvited { get; init; }
}

public sealed class InvitedBy
{
    public required ClickUpId Id { get; init; }
    public required string? Color { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Initials { get; init; }
    
    [JsonPropertyName("profilePicture")]
    public required string? ProfilePicture { get; init; }
}

public sealed class Shared
{
    public required IEnumerable<string> Tasks { get; init; }
    public required IEnumerable<string> Lists { get; init; }
    public required IEnumerable<string> Folders { get; init; }
}

public sealed class CustomRole
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}