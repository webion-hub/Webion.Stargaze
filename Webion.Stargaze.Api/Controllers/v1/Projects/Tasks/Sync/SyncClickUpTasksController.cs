using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.ClickUp.Sync.Synchronization;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Tasks.Sync;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId:guid}/tasks/sync")]
[ApiVersion("1.0")]
[Tags("Projects")]
public class SyncClickUpTasksController : ControllerBase
{
    private readonly ClickUpProjectTasksSynchronizer _synchronizer;
    public SyncClickUpTasksController(ClickUpProjectTasksSynchronizer synchronizer)
    {
        _synchronizer = synchronizer;
    }

    [HttpPut]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sync(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var result = await _synchronizer.SyncAsync(projectId, cancellationToken);

        if (!result)
            NotFound();

        return Ok();
    }
}