using Webion.ClickUp.Api.OAuth;
using Webion.ClickUp.Api.Team;

namespace Webion.ClickUp.Api;

public interface IClickUpApi
{
    public IClickUpTeamApi Teams { get; }
    public IClickUpOAuthApi OAuth { get; }
}