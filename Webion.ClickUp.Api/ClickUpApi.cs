using Webion.ClickUp.Api.Team;

namespace Webion.ClickUp.Api;

public sealed class ClickUpApi
{
    public IClickUpTeamApi Teams { get; init; }

    public ClickUpApi (IClickUpTeamApi teams)
    {
        Teams = teams;
    }
}