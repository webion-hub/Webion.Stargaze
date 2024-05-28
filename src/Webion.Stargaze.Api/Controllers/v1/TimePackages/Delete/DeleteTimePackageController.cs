using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class DeleteTimePackageController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteTimePackageController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Delete time package
    /// </summary>
    /// <remarks>
    /// Deletes a time package.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid timePackageId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.TimePackages
            .Where(x => x.Id == timePackageId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}