using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/tasks/{taskId}")]
[Tags("Tasks")]
[ApiVersion("1.0")]
public sealed class DeleteTaskController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteTaskController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Delete task
    /// </summary>
    /// <remarks>
    /// Deletes a task.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid taskId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.Tasks
            .Where(x => x.Id == taskId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}