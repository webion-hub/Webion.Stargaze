using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Core.Entities;
using Webion.Stargaze.Core.Enums;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Objects.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/clickup/objects")]
[ApiVersion("1.0")]
[Tags("ClickUp")]
public sealed class GetAllClickUpObjectsController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllClickUpObjectsController(StargazeDbContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Get all objects
    /// </summary>
    /// <remarks>
    /// Returns all the clickup objects.
    /// That are spaces, lists and folders.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllClickUpObjectsResponse>(200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var spaces = await _db.ClickUpSpaces
            .Select(x => new ClickUpObjectDto
            {
                Id = new ClickUpObjectId(x.Id, ClickUpObjectType.Space),
                Type = ClickUpObjectType.Space,
                Path = x.Name,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var folders = await _db.ClickUpFolders
            .Select(x => new ClickUpObjectDto
            {
                Id = new ClickUpObjectId(x.Id, ClickUpObjectType.Folder),
                Type = ClickUpObjectType.Folder,
                Path = $"{x.Space.Name} / {x.Name}",
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var lists = await _db.ClickUpLists
            .Where(x => x.FolderId == null)
            .Select(x => new ClickUpObjectDto
            {
                Id = new ClickUpObjectId(x.Id, ClickUpObjectType.List),
                Type = ClickUpObjectType.List,
                Path = $"{x.Space.Name} / {x.Name}",
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var folderLists = await _db.ClickUpLists
            .Where(x => x.FolderId != null)
            .Select(x => new ClickUpObjectDto
            {
                Id = new ClickUpObjectId(x.Id, ClickUpObjectType.List),
                Type = ClickUpObjectType.List,
                Path = $"{x.Space.Name} / {x.Folder!.Name} / {x.Name}",
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(new GetAllClickUpObjectsResponse
        {
            Objects = spaces
                .Concat(folders)
                .Concat(lists)
                .Concat(folderLists)
                .OrderBy(x => x.Path),
        });
    }
}