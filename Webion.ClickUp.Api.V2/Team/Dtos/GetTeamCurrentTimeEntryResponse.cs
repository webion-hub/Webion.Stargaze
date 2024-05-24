using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Team.Dtos;

public sealed class GetTeamCurrentTimeEntryResponse
{
    public Datum2Dto? Data { get; init; }
}