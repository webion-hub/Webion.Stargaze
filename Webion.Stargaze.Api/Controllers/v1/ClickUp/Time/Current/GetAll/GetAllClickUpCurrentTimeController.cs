using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Time.Current.GetAll;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/time/current")]
[Tags("ClickUp")]
[ApiVersion("1.0")]
public sealed class GetAllClickUpCurrentTimeController : ControllerBase
{
    private readonly IClickUpApi _api;
    private readonly StargazeDbContext _db;
    private readonly ClickUpSettings _clickUpSettings;

    public GetAllClickUpCurrentTimeController(IClickUpApi api, StargazeDbContext db, IOptions<ClickUpSettings> clickUpSettings)
    {
        _api = api;
        _db = db;
        _clickUpSettings = clickUpSettings.Value;
    }

    /// <summary>
    /// Get all current time entries
    /// </summary>
    /// <remarks>
    /// Return all current time entries.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllClickUpCurrentTimeResponse>(200)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var teamsResponse = await _api.Teams.GetAllAsync();
        var team = teamsResponse.Teams.First(x => x.Id == _clickUpSettings.TeamId);

        var assignees = team.Members
            .Select(x => x.User.Id)
            .Distinct();

        var requests = assignees
            .Select(x => _api.Teams.GetCurrentTimeEntryAsync(team.Id, x));
        var responses = await Task.WhenAll(requests);

        var timeEntries = responses
            .Where(x => x.Data is not null)
            .Select(x => x.Data!);

        var results = new List<ClickUpCurrentTimeEntryDto>();

        foreach (var timeEntry in timeEntries)
        {
            var task = await _db.Tasks
                .Where(x => x.ClickUpTask != null)
                .If(timeEntry.Task is not null, b => b
                    .Where(x => timeEntry.Task!.Id == x.ClickUpTask!.Id))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var user = await _db.UserLogins
                .Where(x => timeEntry.User.Id == x.ProviderKey)
                .Select(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            results.Add(new ClickUpCurrentTimeEntryDto
            {
                Description = timeEntry.Description,
                Billable = timeEntry.Billable,
                Start = timeEntry.Start.LocalDateTime,
                Duration = DateTimeOffset.Now - timeEntry.Start.LocalDateTime,
                TaskId = task?.Id,
                TaskName = task?.Title,
                TaskDescription = task?.Description,
                UserUsername = user?.UserName,
                UserEmail = user?.Email,
            });
        }

        return Ok(results);
    }
}