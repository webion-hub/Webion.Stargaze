using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.ClickUp.Sync.Synchronization.Impl;

internal sealed class ClickUpListsSynchronizer
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public ClickUpListsSynchronizer(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }
    
    public async Task SynchronizeAsync(CancellationToken cancellationToken)
    {
        await SyncSpaceListsAsync(cancellationToken);
        await SyncFolderListsAsync(cancellationToken);
    }

    private async Task SyncSpaceListsAsync(CancellationToken cancellationToken)
    {
        var spaces = await _db.ClickUpSpaces
            .Include(x => x.Lists
                .Where(f => f.FolderId == null)
            )
            .ToListAsync(cancellationToken);

        var requests = spaces.Select(x => _api.Lists.GetNotInFolderAsync(x.Id));
        var responses = await Task.WhenAll(requests);
        
        foreach (var (space, response) in spaces.Zip(responses))
        {
            space.Lists.SoftReplace(
                replacement: response.Lists,
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
            .Include(x => x.Lists
                .Where(f => f.FolderId != null)
            )
            .ToListAsync(cancellationToken);

        var requests = folders.Select(x => _api.Lists.GetInFolderAsync(x.Id));
        var responses = await Task.WhenAll(requests);

        foreach (var (folder, response) in folders.Zip(responses))
        {
            folder.Lists.SoftReplace(
                replacement: response.Lists,
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