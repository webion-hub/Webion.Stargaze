using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Core.Entities;
using Webion.Stargaze.Core.Enums;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.Stargaze.Services.Link;

public sealed class ClickUpLinkerService
{
    private readonly StargazeDbContext _db;

    public ClickUpLinkerService(StargazeDbContext db)
    {
        _db = db;
    }

    public async Task<bool> LinkAsync(Guid projectId, List<ClickUpObjectId> clickUpObjectIds, CancellationToken cancellationToken)
    {
        var project = await _db.Projects
            .Include(x => x.ClickUpSpaces)
            .Include(x => x.ClickUpLists)
            .Include(x => x.ClickUpFolders)
            .Where(x => x.Id == projectId)
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            return false;

        var clickUpSpaces = await GetClickUpObjects(_db.ClickUpSpaces, clickUpObjectIds, ClickUpObjectType.Space, cancellationToken);
        var clickUpLists = await GetClickUpObjects(_db.ClickUpLists, clickUpObjectIds, ClickUpObjectType.List, cancellationToken);
        var clickUpFolders = await GetClickUpObjects(_db.ClickUpFolders, clickUpObjectIds, ClickUpObjectType.Folder, cancellationToken);

        project.ClickUpSpaces.SoftReplace(
            replacement: clickUpSpaces,
            match: (o, n) => o.Id == n.Id,
            add: n => n,
            update: (_, _) => { },
            delete: (n) => n.Projects.Remove(project)
        );

        project.ClickUpLists.SoftReplace(
            replacement: clickUpLists,
            match: (o, n) => o.Id == n.Id,
            add: n => n,
            update: (_, _) => { },
            delete: (n) => n.Projects.Remove(project)
        );

        project.ClickUpFolders.SoftReplace(
            replacement: clickUpFolders,
            match: (o, n) => o.Id == n.Id,
            add: n => n,
            update: (_, _) => { },
            delete: (n) => n.Projects.Remove(project)
        );

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static async Task<IEnumerable<T>> GetClickUpObjects<T>(
        DbSet<T> dbSet,
        List<ClickUpObjectId> ids,
        ClickUpObjectType type,
        CancellationToken cancellationToken
    ) where T : class, IClickUpObject
    {
        var typedClickUpObjectsIds = ids
            .Where(x => x.Type == type)
            .Select(x => x.Id)
            .ToList();

        var result = await dbSet
            .Include(x => x.Projects)
            .Where(x => typedClickUpObjectsIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return result;
    }
}