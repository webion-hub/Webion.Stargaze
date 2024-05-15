namespace Webion.ClickUp.Api.V2.Common.Dtos;

public class CreatorDto
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    public required string Color { get; init; }
    public required string ProfilePicture { get; init; }
}