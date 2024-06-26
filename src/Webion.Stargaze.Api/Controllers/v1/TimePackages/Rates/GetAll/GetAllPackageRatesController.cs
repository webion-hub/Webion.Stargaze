using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}/rates")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class GetAllPackageRatesController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllPackageRatesController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all time package rates
    /// </summary>
    /// <remarks>
    /// Get all time package rates filtered by user.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllPackageRatesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid timePackageId,
        [FromQuery] GetAllPackageRatesRequest request,
        CancellationToken cancellationToken
    )
    {
        var packageRates = await _db.TimePackageRates
            .Where(x => x.TimePackageId == timePackageId)
            .If(request.UserId is not null, b => b
                .Where(x => x.UserId == request.UserId)
            )
            .Select(x => new TimePackageRateDto
            {
                Id = x.Id,
                UserId = x.UserId,
                PackageId = x.TimePackageId,
                Rate = x.Rate,

            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(packageRates);
    }
}