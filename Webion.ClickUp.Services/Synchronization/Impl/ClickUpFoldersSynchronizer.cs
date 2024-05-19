using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.ClickUp.Sync.Synchronization.Impl;

internal sealed class ClickUpFoldersSynchronizer
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public ClickUpFoldersSynchronizer(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task SynchronizeAsync(CancellationToken cancellationToken)
    {
        var spaces = await _db.ClickUpSpaces
            .Include(x => x.Folders)
            .ToListAsync(cancellationToken);

        var requests = spaces.Select(x => _api.Folders.GetAllAsync(x.Id, null));
        var responses = await Task.WhenAll(requests);
        
        foreach (var (space, response) in spaces.Zip(responses))
        {
            space.Folders.SoftReplace(
                replacement: response.Folders,
                match: (o, n) => o.Id == n.Id,
                add: n => new ClickUpFolderDbo
                {
                    Id = n.Id,
                    SpaceId = n.Space.Id,
                    Name = n.Name
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