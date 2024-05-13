using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.Team.Dtos;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Time;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/time")]
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]
public sealed class SyncClickUpTrackedTimeController : ControllerBase
{
    private readonly IClickUpApi _clickUpApi;
    private readonly StargazeDbContext _db;
    private readonly ClickUpSettings _clickUpSettings;

    public SyncClickUpTrackedTimeController(IClickUpApi clickUpApi, ClickUpSettings clickUpSettings, StargazeDbContext db)
    {
        _clickUpApi = clickUpApi;
        _clickUpSettings = clickUpSettings;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Sync()
    {
        var teamsResponse = await _clickUpApi.Teams.GetAllAsync();
        var team = teamsResponse.Teams.First(x => x.Id == _clickUpSettings.TeamId);
        
        var request = new GetTeamTimeEntriesRequest
        {
            Assignee = team.Members
                .Select(x => x.User.Id)
                .Distinct()
        };
            
        var response = await _clickUpApi.Teams.GetTimeEntriesAsync(team.Id, request);

        _db.TimeEntries.AddRange(response.Data.Select(x => new TimeEntryDbo
        {
            Id = default,
            UserId = default,
            TaskId = null,
            Start = default,
            End = default,
            Duration = default,
            Locked = false,
            Billable = false,
            Billed = false,
            Paid = false,
            User = null!,
            Task = null,
        }));
        
        return Ok(response.Data);
    }
}