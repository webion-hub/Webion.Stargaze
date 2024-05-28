using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/tasks/{taskId}")]
[Tags("Tasks")]
[ApiVersion("1.0")]
public sealed class GetTaskController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetTaskController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get task
    /// </summary>
    /// <remarks>
    /// Returns a task.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetTaskResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid taskId,
        CancellationToken cancellationToken
    )
    {
        var response = await _db.Tasks
            .Where(x => x.Id == taskId)
            .Select(x => new GetTaskResponse
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Title = x.Title,
                Description = x.Description,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}