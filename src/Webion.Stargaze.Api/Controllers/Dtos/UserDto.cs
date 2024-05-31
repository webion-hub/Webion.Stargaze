namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class UserDto
{
    public required Guid UserId { get; init; }
    public required string? UserName { get; init; }
    public required string? Email { get; init; }
    public required string? PhoneNumber { get; init; }
}