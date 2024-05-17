using Refit;
using Webion.ClickUp.Api.V2.Tasks.Dtos;

namespace Webion.ClickUp.Api.V2.Tasks;

public interface IClickUpTasksApi
{
    [Get("/v2/list/{list_id}/task")]
    Task<GetAllTasksResponse> GetAllAsync([AliasAs("list_id")] long listId, [Query] GetAllTasksRequest request);
}