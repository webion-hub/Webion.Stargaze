using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.Team.Dtos;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Time;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/time")]
[Tags("ClickUp")]
[ApiVersion("1.0")]
public sealed class SyncClickUpTrackedTimeController : ControllerBase
{
    private readonly IClickUpApi _clickUpApi;
    private readonly StargazeDbContext _db;
    private readonly ClickUpSettings _clickUpSettings;

    public SyncClickUpTrackedTimeController(IClickUpApi clickUpApi, IOptions<ClickUpSettings> clickUpSettings, StargazeDbContext db)
    {
        _clickUpApi = clickUpApi;
        _clickUpSettings = clickUpSettings.Value;
        _db = db;
    }

    /// <summary>
    /// Synchronize time entries
    /// </summary>
    /// <remarks>
    /// Synchronizes all time entries.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
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

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var users = await _db.UserLogins
            .Where(x => x.LoginProvider == ClickUpDefaults.AuthenticationScheme)
            .AsNoTracking()
            .ToDictionaryAsync(
                keySelector: x => x.ProviderKey,
                elementSelector: x => x.UserId,
                cancellationToken: cancellationToken
            );

        var chunks = response.Data.Chunk(1000);
        foreach (var chunk in chunks)
        {
            _db.TimeEntries.AddRange(chunk.Select(x => new TimeEntryDbo
            {
                UserId = users[x.User.Id],
                TaskId = null,
                Start = x.Start,
                Description = x.Description,
                End = x.End,
                Duration = x.Duration,
                Locked = false,
                Billable = x.Billable,
                Billed = false,
            }));

            await _db.SaveChangesAsync(cancellationToken);
        }
        
        await transaction.CommitAsync(cancellationToken);
        return Ok();
    }
}