using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
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
    private readonly AddPackageRateRequestValidator _requestValidator;

    public AddPackageRateController(StargazeDbContext db, AddPackageRateRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPost]
    [ProducesResponseType<AddPackageRateResponse>(201)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid timePackageId,
        [FromBody] AddPackageRateRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var timePackageExist = await _db.TimePackages
            .AnyAsync(x => x.Id == timePackageId, cancellationToken);

        if (!timePackageExist)
            return NotFound();

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