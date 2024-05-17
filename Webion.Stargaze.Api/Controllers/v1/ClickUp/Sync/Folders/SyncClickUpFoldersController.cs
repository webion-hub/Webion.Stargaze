using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Folders;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/folders")]
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]
public sealed class SyncClickUpFoldersController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public SyncClickUpFoldersController(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var spaces = await _db.ClickUpSpaces
            .Include(x => x.Folders)
            .ToListAsync(cancellationToken);

        foreach (var space in spaces)
        {
            var tasksResponse = await _api.Folders.GetAllAsync(Convert.ToInt64(space.Id), null!);

            space.Folders.SoftReplace(
                replacement: tasksResponse.Folders,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpFolderDbo
                {
                    Id = n.Id,
                    SpaceId = n.Space.Id
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