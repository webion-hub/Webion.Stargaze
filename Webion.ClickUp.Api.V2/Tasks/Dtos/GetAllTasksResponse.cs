using Webion.ClickUp.Api.V2.Common.Dtos;

namespace Webion.ClickUp.Api.V2.Tasks.Dtos;

public partial class GetAllTasksResponse
{
    public IEnumerable<Task9> Tasks { get; init; } = [];
    public bool Last_Page { get; init; }
}