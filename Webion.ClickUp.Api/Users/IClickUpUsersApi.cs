using Refit;
using Webion.ClickUp.Api.Users.Dtos;

namespace Webion.ClickUp.Api.Users;

public interface IClickUpUsersApi
{
    [Get("/v2/team/{team_id}/user/{user_id}")]
    Task<GetUserResponse> GetAsync(
        [AliasAs("team_id")] long teamId,
        [AliasAs("user_id")] long userId
    );
}