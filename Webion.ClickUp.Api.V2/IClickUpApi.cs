using Webion.ClickUp.Api.V2.OAuth;
using Webion.ClickUp.Api.V2.Team;
using Webion.ClickUp.Api.V2.Users;

namespace Webion.ClickUp.Api.V2;

public interface IClickUpApi
{
    public IClickUpTeamApi Teams { get; }
    public IClickUpOAuthApi OAuth { get; }
    public IClickUpUsersApi Users { get; }
}