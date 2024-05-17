using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Lists;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/lists")]
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]

public sealed class SyncClickUpListsController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public SyncClickUpListsController(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        
        await SyncSpaceListsAsync(cancellationToken);
        await SyncFolderListsAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return Ok();
    }

    private async Task SyncSpaceListsAsync(CancellationToken cancellationToken)
    {
        var spaces = await _db.ClickUpSpaces
            .Include(x => x.Lists)
            .ToListAsync(cancellationToken);

        foreach (var space in spaces)
        {
            var listsResponse = await _api.Lists.GetNotInFolderAsync(space.Id);

            space.Lists.SoftReplace(
                replacement: listsResponse.Lists,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpListDbo
                {
                    Id = n.Id,
                    SpaceId = n.Space.Id,
                    Name = n.Name,
                },
                update: (o, n) =>
                {
                    o.Name = n.Name;
                },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task SyncFolderListsAsync(CancellationToken cancellationToken)
    {
        var folders = await _db.ClickUpFolders
            .Include(x => x.Lists)
            .ToListAsync(cancellationToken);

        foreach (var folder in folders)
        {
            var listsResponse = await _api.Lists.GetInFolderAsync(folder.Id);

            folder.Lists.SoftReplace(
                replacement: listsResponse.Lists,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpListDbo
                {
                    Id = n.Id,
                    SpaceId = n.Space.Id,
                    Name = n.Name,
                },
                update: (o, n) =>
                {
                    o.Name = n.Name;
                },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}