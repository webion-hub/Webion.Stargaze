namespace Webion.ClickUp.Api.V2.Common;

public sealed class TeamDto
{
    public required ClickUpId Id { get; init; }
    public required string Name { get; init; }
    public required string Color { get; init; }
    public required string Avatar { get; init; }
    public required ICollection<MemberDto> Members { get; init; }
}