using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}/rates/{packageRateId}")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class UpdatePackageRateController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public UpdatePackageRateController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid timePackageId,
        [FromRoute] Guid packageRateId,
        [FromBody] UpdatePackageRateRequest request,
        CancellationToken cancellationToken
    )
    {
        var updatedRows = await _db.TimePackageRates
            .Where(x => x.TimePackageId == timePackageId)
            .Where(x => x.Id == packageRateId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.Rate, request.Rate)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}