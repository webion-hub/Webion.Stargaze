using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Tasks.Dtos;

public partial class GetAllTasksResponse
{
    public IEnumerable<Task9Dto> Tasks { get; init; } = [];
    public bool Last_Page { get; init; }
}