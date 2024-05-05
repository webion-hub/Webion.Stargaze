using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.ClickUp.Api;
using Webion.ClickUp.Api.Team.Dtos;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.TimeTracking.ClickUp.Sync;

[ApiController]
[Authorize]
[Route("v1/time-tracking/clickup/sync/{teamId:long}")]
public sealed class SyncClickUpTrackedTimeController : ControllerBase
{
    private readonly IClickUpApi _clickUpApi;
    private readonly StargazeDbContext _db;
    private readonly UserManager<UserDbo> _userManager;

    public SyncClickUpTrackedTimeController(IClickUpApi clickUpApi, StargazeDbContext db, UserManager<UserDbo> userManager)
    {
        _clickUpApi = clickUpApi;
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Sync([FromRoute] long teamId)
    {
        var user = await _userManager.GetUserAsync(User);
        
        var teamsResponse = await _clickUpApi.Teams.GetAllAsync();
        var response = await _clickUpApi.Teams.GetTimeEntriesAsync(teamId, new GetTeamTimeEntriesRequest
        {
            Assignee = teamsResponse.Teams
                .SelectMany(x => x.Members)
                .Select(x => x.User.Id)
                .Distinct()
        });

        return Ok(response.Data);
    }
}