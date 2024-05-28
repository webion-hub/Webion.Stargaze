using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Remove;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}/rates/{packageRateId}")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class RemovePackageRateController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public RemovePackageRateController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Remove time package rate
    /// </summary>
    /// <remarks>
    /// Removes a time package rate.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid timePackageId,
        [FromRoute] Guid packageRateId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.TimePackageRates
            .Where(x => x.TimePackageId == timePackageId)
            .Where(x => x.Id == packageRateId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}