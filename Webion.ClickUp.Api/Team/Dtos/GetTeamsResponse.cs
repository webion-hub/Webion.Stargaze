using Webion.ClickUp.Api.Common;

namespace Webion.ClickUp.Api.Team.Dtos;

public sealed class GetTeamsResponse
{
    public required IEnumerable<TeamDto> Teams { get; init; } = [];
}