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
        var durations = await _db.TimeEntries
            .Where(x => !x.Billed)
            .Select(x => x.Duration)
            .ToListAsync(cancellationToken);

        var totalTime = durations.Aggregate(TimeSpan.Zero, (p, c) => p + c);

        var timePackages = await _db.TimePackages
            .If(request.CompanyId is not null, b => b)
            .Select(x => new TimePackageDto
            {
                Id = x.Id,
                TotalHours = x.Hours,
                Name = x.Name
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        foreach (var package in timePackages.Results)
        {
            totalTime -= TimeSpan.FromHours(package.TotalHours);
            package.RemainingHours = totalTime.Hours > 0 ? 0 : package.TotalHours + totalTime.Hours;
        }

        return Ok(GetAllTimePackagesResponse.From(timePackages));
    }
}