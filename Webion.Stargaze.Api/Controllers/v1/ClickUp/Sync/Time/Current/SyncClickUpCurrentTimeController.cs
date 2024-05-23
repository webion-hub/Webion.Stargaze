using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.ClickUp.Api.V2;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Time.Current;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/time/current")]
[Tags("ClickUp")]
[ApiVersion("1.0")]
public sealed class SyncClickUpCurrentTimeController : ControllerBase
{
    private readonly IClickUpApi _api;
    private readonly StargazeDbContext _db;
    private readonly ClickUpSettings _clickUpSettings;

    public SyncClickUpCurrentTimeController(IClickUpApi api, IOptions<ClickUpSettings> clickUpSettings, StargazeDbContext db)
    {
        _api = api;
        _clickUpSettings = clickUpSettings.Value;
        _db = db;
    }

    /// <summary>
    /// Synchronize current time entries
    /// </summary>
    /// <remarks>
    /// Synchronizes all current time entries.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(200)]
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
            .Select(x => x.Data);

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var users = await _db.UserLogins
            .Where(x => x.LoginProvider == ClickUpDefaults.AuthenticationScheme)
            .AsNoTracking()
            .ToDictionaryAsync(
                keySelector: x => x.ProviderKey,
                elementSelector: x => x.UserId,
                cancellationToken: cancellationToken
            );

        var chunks = timeEntries.Chunk(1000);
        foreach (var chunk in chunks)
        {
            _db.TimeEntries.AddRange(chunk.Select(x => new TimeEntryDbo
            {
                UserId = users[x!.User.Id],
                TaskId = null,
                Start = x.Start,
                End = DateTimeOffset.MaxValue,
                Description = x.Description,
                Duration = DateTimeOffset.Now - x.Start,
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