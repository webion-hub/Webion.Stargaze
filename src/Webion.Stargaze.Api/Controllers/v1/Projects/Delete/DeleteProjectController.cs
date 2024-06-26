using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId}")]
[Tags("Projects")]
[ApiVersion("1.0")]
public sealed class DeleteProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteProjectController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Delete project
    /// </summary>
    /// <remarks>
    /// Deletes a project.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.Projects
            .Where(x => x.Id == projectId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}