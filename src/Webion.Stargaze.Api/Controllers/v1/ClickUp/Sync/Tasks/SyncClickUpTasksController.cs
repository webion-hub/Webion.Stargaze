using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Tasks;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/tasks")]
[ApiVersion("1.0")]
[Tags("ClickUp")]
public sealed class SyncClickUpTasksController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public SyncClickUpTasksController(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    /// <summary>
    /// Synchronize tasks
    /// </summary>
    /// <remarks>
    /// Synchronizes all clickup tasks.<br/>
    /// It works on the lists that were imported using the <c>v1/clickup/sync/objects</c> endpoint.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var lists = await _db.ClickUpLists
            .Include(x => x.Tasks)
            .ToListAsync(cancellationToken);

        var requests = lists.Select(x => _api.Tasks.GetAllAsync(x.Id));
        var responses = await Task.WhenAll(requests);

        foreach (var (list, response) in lists.Zip(responses))
        {
            list.Tasks.SoftReplace(
                replacement: response.Tasks,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpTaskDbo
                {
                    Id = n.Id,
                    ListId = n.List.Id,
                    Title = n.Name,
                    Description = n.Description
                },
                update: (o, n) =>
                {
                    o.Title = n.Name;
                    o.Description = n.Description;
                },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        return Ok();
    }
}