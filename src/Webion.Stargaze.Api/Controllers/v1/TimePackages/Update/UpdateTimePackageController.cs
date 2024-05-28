using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages/{timePackageId}")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class UpdateTimePackageController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdateTimePackageRequestValidator _requestValidator;

    public UpdateTimePackageController(StargazeDbContext db, UpdateTimePackageRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Update time package
    /// </summary>
    /// <remarks>
    /// Updates a time package.
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid timePackageId,
        [FromBody] UpdateTimePackageRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var timePackage = await _db.TimePackages
            .Where(x => x.Id == timePackageId)
            .FirstOrDefaultAsync(cancellationToken);

        if (timePackage is null)
            return NotFound();

        var projects = await _db.Projects
            .Where(x => request.AppliesToProjects.Contains(x.Id))
            .ToListAsync(cancellationToken);

        timePackage.CompanyId = request.CompanyId;
        timePackage.Hours = request.Hours;
        timePackage.Name = request.Name;
        timePackage.AppliesTo = projects;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}