using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages")]
[Tags("Time Packages")]
public class CreateTimePackageController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public CreateTimePackageController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [ProducesResponseType<CreateTimePackageResponse>(201)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTimePackageRequest request,
        CancellationToken cancellationToken
    )
    {
        var projects = await _db.Projects
            .Where(x => request.AppliesToProjects.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var package = new TimePackageDbo
        {
            Hours = request.Hours,
            Name = request.Name,
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