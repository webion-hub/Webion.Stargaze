using Webion.ClickUp.Api.OAuth;
using Webion.ClickUp.Api.Team;
using Webion.ClickUp.Api.Users;

namespace Webion.ClickUp.Api;

public interface IClickUpApi
{
    public IClickUpTeamApi Teams { get; }
    public IClickUpOAuthApi OAuth { get; }
    public IClickUpUsersApi Users { get; }
}