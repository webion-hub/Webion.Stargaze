using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Api.Controllers.Entities;
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

    /// <summary>
    /// Get all time packages
    /// </summary>
    /// <remarks>
    /// Returns all the time packages filtered by company.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllTimePackagesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTimePackagesRequest request,
        CancellationToken cancellationToken
    )
    {
        var timePackages = await _db.TimePackages
            .Include(x => x.Rates)
            .Include(x => x.AppliesTo)
            .If(request.CompanyId is not null, b => b
                .Where(x => x.CompanyId == request.CompanyId)
            )
            .AsSplitQuery()
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(cancellationToken);

        var projectsIds = timePackages
            .SelectMany(x => x.AppliesTo)
            .Select(x => x.Id)
            .Distinct()
            .ToList();

        var usersIds = timePackages
            .SelectMany(x => x.Rates)
            .Select(x => x.UserId)
            .Distinct()
            .ToList();

        var entries = await _db.TimeEntries
            .Where(x => x.Task != null)
            .Where(x => x.Billable)
            .Where(x => !x.Billed)
            .Where(x => projectsIds.Contains(x.Task!.ProjectId))
            .Where(x => usersIds.Contains(x.UserId))
            .OrderBy(x => x.Start)
            .Select(x => new EntryDetailEntity(
                x.Task!.Project,
                x.User,
                x.Duration
            ))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(cancellationToken);

        var totalTime = entries
            .Sum(x => x.TrackedTime.TotalHours);

        var dtos = new List<TimePackageDto>();
        foreach (var package in timePackages)
        {
            var dto = new TimePackageDto
            {
                Id = package.Id,
                CompanyId = package.CompanyId,
                Name = package.Name,
                TotalHours = package.Hours,
                RemainingHours = package.Hours,
                TrackedHours = 0,
                Earnings = 0,
                Cost = 0,
                AppliesTo = package.AppliesTo.Select(x => x.Id),
                Users = package.Rates.Select(x => x.UserId),
            };

            foreach (var i in Enumerable.Range(0, 10_000))
            {
                if (dto.TrackedHours >= package.Hours)
                    break;

                var entry = entries
                    .Where(x => dto.AppliesTo.Contains(x.Project.Id))
                    .Where(x => dto.Users.Contains(x.User.Id))
                    .FirstOrDefault();

                if (entry is null)
                    break;

                var userRate = package.Rates
                    .Where(x => x.UserId == entry.User.Id)
                    .Select(x => x.Rate)
                    .FirstOrDefault();

                var remainingHours = dto.RemainingHours;
                var applicableHours = TimeSpan.FromHours(
                    Math.Min(entry.TrackedTime.TotalHours, remainingHours)
                );

                dto.TrackedHours += applicableHours.TotalHours;
                dto.RemainingHours = package.Hours - dto.TrackedHours;
                dto.Earnings += (decimal)applicableHours.TotalHours * userRate;

                entry.TrackedTime -= applicableHours;

                if (entry.TrackedTime <= TimeSpan.Zero)
                    entries.Remove(entry);
            }

            dtos.Add(dto);
        }

        var remainingBillableTime = entries
            .Sum(x => x.TrackedTime.TotalHours);

        return Ok(new GetAllTimePackagesResponse
        {
            TimePackages = dtos,
            TotalTime = totalTime,
            RemainingBillableTime = remainingBillableTime
        });
    }
}