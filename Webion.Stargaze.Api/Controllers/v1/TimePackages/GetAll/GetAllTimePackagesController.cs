using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/packages")]
[Tags("Time Packages")]
[ApiVersion("1.0")]
public sealed class GetAllTimePackagesController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllTimePackagesController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllTimePackagesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTimePackagesRequest request,
        CancellationToken cancellationToken
    )
    {
        var timePackages = await _db.TimePackages
            .If(request.CompanyId is not null, b => b
                .Where(x => x.CompanyId == request.CompanyId)
            )
            .Where(x => x.AppliesTo.Count != 0)
            .Select(x => new TimePackageDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Name = x.Name,
                TotalHours = x.Hours,
                RemainingHours = x.Hours,
                TrackedHours = 0,
                AppliesTo = x.AppliesTo
                    .Select(x => x.Id)
            })
            .ToListAsync(cancellationToken);

        var projectsIds = timePackages
            .SelectMany(x => x.AppliesTo)
            .ToList();

        var trackedTimes = await _db.TimeEntries
            .Where(x => x.Task != null)
            .Where(x => x.Billable == true)
            .Where(x => x.Billed == false)
            .Where(x => projectsIds
                .Contains(x.Task!.ProjectId)
            )
            .GroupBy(x => x.Task!.ProjectId)
            .Select(x => new TrackedTimeDto
            {
                ProjectId = x.Key,
                TotalHours = x
                    .Sum(x => x.Duration.Hours)
            })
            .ToDictionaryAsync(
                keySelector: x => x.ProjectId,
                elementSelector: x => x.TotalHours,
                cancellationToken: cancellationToken);

        foreach (var t in timePackages)
        {
            foreach (var project in t.AppliesTo)
            {
                t.TrackedHours += trackedTimes[project] > t.TotalHours
                    ? t.TotalHours
                    : trackedTimes[project];
                t.RemainingHours -= t.TrackedHours;

                trackedTimes[project] -= t.TrackedHours;
            }
        }

        return Ok(new GetAllTimePackagesResponse
        {
            TimePackages = timePackages
        });
    }
}