using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.Common;
using Webion.ClickUp.Sync.Synchronization.Impl;
using Webion.Stargaze.Pgsql;

namespace Webion.ClickUp.Sync.Synchronization;

public sealed class ClickUpObjectsSynchronizer
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public ClickUpObjectsSynchronizer(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task SynchronizeAsync(ClickUpId teamId, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        await new ClickUpSpacesSynchronizer(_db, _api).SynchronizeAsync(teamId, cancellationToken);
        await new ClickUpFoldersSynchronizer(_db, _api).SynchronizeAsync(cancellationToken);
        await new ClickUpListsSynchronizer(_db, _api).SynchronizeAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}