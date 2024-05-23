using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api.V2;
using Webion.Stargaze.Api.Options;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Time.Current.GetAll;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/time/current")]
[Tags("ClickUp")]
[ApiVersion("1.0")]
public sealed class GetAllClickUpCurrentTimeController : ControllerBase
{
    private readonly IClickUpApi _api;
    private readonly ClickUpSettings _clickUpSettings;

    public GetAllClickUpCurrentTimeController(IClickUpApi api, IOptions<ClickUpSettings> clickUpSettings)
    {
        _api = api;
        _clickUpSettings = clickUpSettings.Value;
    }

    /// <summary>
    /// Get all current time entries
    /// </summary>
    /// <remarks>
    /// Return all current time entries.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sync()
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
            .Select(x => x.Data);

        return Ok(timeEntries);
    }
}