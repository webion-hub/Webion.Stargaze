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
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]
public sealed class SyncClickUpTasksController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public SyncClickUpTasksController(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var lists = await _db.ClickUpLists
            .Include(x => x.Tasks)
            .ToListAsync(cancellationToken);

        foreach (var list in lists)
        {
            var tasksResponse = await _api.Tasks.GetAllAsync(Convert.ToInt64(list.Id), null!);

            list.Tasks.SoftReplace(
                replacement: tasksResponse.Tasks,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpTaskDbo
                {
                    Id = n.Id,
                    ListId = n.List.Id
                },
                update: (o, n) => { },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        return Ok();
    }
}