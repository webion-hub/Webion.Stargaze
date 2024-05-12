using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Team.Dtos;

public sealed class GetTeamsResponse
{
    public required IEnumerable<TeamDto> Teams { get; init; } = [];
}