using Microsoft.EntityFrameworkCore;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.ClickUp.Sync.Synchronization;

public sealed class ClickUpProjectTasksSynchronizer
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;

    public ClickUpProjectTasksSynchronizer(StargazeDbContext db, IClickUpApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<bool> SyncAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _db.Projects
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);

        if (project is null)
            return false;

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var lists = await _db.ClickUpLists
            .Where(x => x.Projects.Any(x => x.Id == projectId)
                || x.Folder!.Projects.Any(x => x.Id == projectId)
                || x.Space.Projects.Any(x => x.Id == projectId)
                || x.Folder.Space.Projects.Any(x => x.Id == projectId)
            )
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
                },
                update: (o, n) => { },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }

        project.Tasks.SoftReplace(
            replacement: lists.SelectMany(x => x.Tasks),
            match: (o, n) => o.Id == n.TaskId,
            add: n => new TaskDbo
            {
                ClickUpTask = n,
                Title = ""
            },
            update: (o, n) => { },
            delete: (n) => { }
        );
        await _db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}