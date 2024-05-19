using Refit;
using Webion.ClickUp.Api.V2.Tasks.Dtos;

namespace Webion.ClickUp.Api.V2.Tasks;

public interface IClickUpTasksApi
{
    [Get("/v2/list/{listId}/task")]
    Task<GetAllTasksResponse> GetAllAsync(string listId, [Query] GetAllTasksRequest? request = null);
}