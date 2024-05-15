using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/entries/{timeEntryId}")]
[Tags("Time Entries")]
[ApiVersion("1.0")]
public sealed class DeleteTimeEntryController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteTimeEntryController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid timeEntryId,
        CancellationToken cancellationToken
    )
    {
        var timeEntries = await _db.TimeEntries
            .Where(x => x.Id == timeEntryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (timeEntries is null)
            return NotFound();

        if (timeEntries.Locked)
            return Problem(
                detail: "Locked time entry",
                statusCode: StatusCodes.Status403Forbidden
            );

        _db.Remove(timeEntries);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}