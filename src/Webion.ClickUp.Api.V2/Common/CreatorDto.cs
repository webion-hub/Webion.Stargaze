namespace Webion.ClickUp.Api.V2.Common;

public class CreatorDto
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    public required string Color { get; init; }
    public required string Email { get; init; }
    public string? ProfilePicture { get; init; }
}