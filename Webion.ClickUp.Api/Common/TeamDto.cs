namespace Webion.ClickUp.Api.Common;

public sealed class TeamDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Color { get; init; }
    public required string Avatar { get; init; }
    public required ICollection<MemberDto> Members { get; init; }
}