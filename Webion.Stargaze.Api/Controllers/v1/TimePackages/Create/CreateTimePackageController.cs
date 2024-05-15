using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public class CreateTimePackageController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateTimePackageRequestValidator _requestValidator;

    public CreateTimePackageController(StargazeDbContext db, CreateTimePackageRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPost]
    [ProducesResponseType<CreateTimePackageResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTimePackageRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var projects = await _db.Projects
            .Where(x => request.AppliesToProjects.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var package = new TimePackageDbo
        {
            CompanyId = request.CompanyId,
            Name = request.Name,
            Hours = request.Hours,
            AppliesTo = projects,
        };

        _db.TimePackages.Add(package);
        await _db.SaveChangesAsync(cancellationToken);

        return Created("v1/time/packages", new CreateTimePackageResponse
        {
            Id = package.Id,
        });
    }
}