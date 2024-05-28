using Webion.ClickUp.Api.V2.Folders;
using Webion.ClickUp.Api.V2.Lists;
using Webion.ClickUp.Api.V2.OAuth;
using Webion.ClickUp.Api.V2.Spaces;
using Webion.ClickUp.Api.V2.Tasks;
using Webion.ClickUp.Api.V2.Team;
using Webion.ClickUp.Api.V2.Users;

namespace Webion.ClickUp.Api.V2;

public interface IClickUpApi
{
    public IClickUpTeamApi Teams { get; }
    public IClickUpOAuthApi OAuth { get; }
    public IClickUpUsersApi Users { get; }
    public IClickUpSpacesApi Spaces { get; }
    public IClickUpListsApi Lists { get; }
    public IClickUpTasksApi Tasks { get; }
    public IClickUpFoldersApi Folders { get; }
}