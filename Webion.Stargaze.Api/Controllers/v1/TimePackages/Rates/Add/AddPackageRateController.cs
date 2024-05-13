using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Add;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}/rates")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class AddPackageRateController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public AddPackageRateController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [ProducesResponseType<AddPackageRateResponse>(201)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid timePackageId,
        [FromBody] CreatePackageRateRequest request,
        CancellationToken cancellationToken
    )
    {
        var rate = new TimePackageRateDbo
        {
            UserId = request.UserId,
            TimePackageId = timePackageId,
            Rate = request.Rate,
        };

        _db.TimePackageRates.Add(rate);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/time/packages/{timePackageId}/rates/{rate.Id}", new AddPackageRateResponse
        {
            Id = rate.Id,
        });
    }
}