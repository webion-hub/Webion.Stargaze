using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.Common;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.ClickUp.Sync.Synchronization.Impl;

internal sealed class ClickUpSpacesSynchronizer
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public ClickUpSpacesSynchronizer(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }
    
    public async Task SynchronizeAsync(ClickUpId teamId, CancellationToken cancellationToken)
    {
        var spacesResponse = await _api.Spaces.GetAllAsync(teamId, null!);
        var spaces = await _db.ClickUpSpaces.ToListAsync(cancellationToken);

        var newSpaceIds = spacesResponse.Spaces
            .Where(x => spaces.All(s => s.Id != x.Id))
            .Select(x => x.Id);
        
        spaces.SoftReplace(
            replacement: spacesResponse.Spaces,
            match: (o, n) => o.Id == n.Id,
            add: n => new ClickUpSpaceDbo
            {
                Id = n.Id,
                Name = n.Name
            },
            update: (o, n) =>
            {
                o.Name = n.Name;
            },
            delete: o => _db.Remove(o)
        );

        var newSpaces = spaces.Where(x => newSpaceIds.Contains(x.Id));
        _db.AddRange(newSpaces);
        
        await _db.SaveChangesAsync(cancellationToken);
    }
}