using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
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
            .Include(x => x.AppliesTo)
            .If(request.CompanyId is not null, b => b
                .Where(x => x.CompanyId == request.CompanyId)
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var linkedProjects = timePackages
            .SelectMany(x => x.AppliesTo)
            .Select(x => x.Id);
            
        var durations = await _db.TimeEntries
            .Where(x => linkedProjects.Contains(x.Task!.ProjectId))
            .Where(x => !x.Billed)
            .Select(x => x.Duration)
            .ToListAsync(cancellationToken);
        
        
        var totalTime = durations.Aggregate(TimeSpan.Zero, (p, c) => p + c);
        var currTime = totalTime;

        var res = new List<TimePackageDto>();
        foreach (var package in timePackages)
        {
            if (currTime <= TimeSpan.Zero)
            {
                res.Add(new TimePackageDto
                {
                    Id = package.Id,
                    TotalHours = package.Hours,
                    Name = package.Name,
                    RemainingHours = package.Hours,
                    TrackedHours = 0,
                });
                
                break;
            }
            
            var totalHours = TimeSpan.FromHours(package.Hours);
            var diffTime = totalHours - currTime;
            var remainingTime = diffTime < TimeSpan.Zero
                ? totalHours
                : diffTime;

            res.Add(new TimePackageDto
            {
                Id = package.Id,
                TotalHours = package.Hours,
                Name = package.Name,
                RemainingHours = remainingTime.TotalHours,
                TrackedHours = (totalHours - remainingTime).TotalHours,
            });

            currTime -= remainingTime;
        }

        return Ok(new
        {
            TotalTime = totalTime,
            RemainingBillableTime = currTime < TimeSpan.Zero ? 0 : currTime.TotalHours,
            Packages = res,
        });
    }
}