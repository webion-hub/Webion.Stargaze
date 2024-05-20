using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Core.Entities;
using Webion.Stargaze.Core.Enums;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Services.Link;

public sealed class ClickUpLinkerService
{
    private readonly StargazeDbContext _db;

    public ClickUpLinkerService(StargazeDbContext db)
    {
        _db = db;
    }

    public async Task<bool> LinkAsync(Guid projectId, List<ClickUpObjectId> clickUpObjectIds)
    {
        var clickUpSpacesIds = clickUpObjectIds
            .Where(x => x.Type == ClickUpObjectType.Space)
            .Select(x => x.Id)
            .ToList();

        var clickUpSpaces = await _db.ClickUpSpaces
            .Include(x => x.Projects)
            .Where(x => clickUpSpacesIds.Contains(x.Id))
            .ToListAsync();

        var clickUpListsIds = clickUpObjectIds
            .Where(x => x.Type == ClickUpObjectType.List)
            .Select(x => x.Id)
            .ToList();

        var clickUpLists = await _db.ClickUpLists
            .Include(x => x.Projects)
            .Where(x => clickUpListsIds.Contains(x.Id))
            .ToListAsync();

        var clickUpFoldersIds = clickUpObjectIds
            .Where(x => x.Type == ClickUpObjectType.Folder)
            .Select(x => x.Id)
            .ToList();

        var clickUpFolders = await _db.ClickUpFolders
            .Include(x => x.Projects)
            .Where(x => clickUpFoldersIds.Contains(x.Id))
            .ToListAsync();

        var project = await _db.Projects
            .Include(x => x.ClickUpSpaces)
            .Include(x => x.ClickUpLists)
            .Include(x => x.ClickUpFolders)
            .Where(x => x.Id == projectId)
            .FirstOrDefaultAsync();

        if (project is null)
            return false;

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

        await _db.SaveChangesAsync();
        return true;
    }
}