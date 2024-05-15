using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api.V2;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Options;
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
    private readonly ClickUpSettings _settings;

    public SyncClickUpListsController(StargazeDbContext db, IClickUpApi api, IOptions<ClickUpSettings> settings)
    {
        _db = db;
        _api = api;
        _settings = settings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var spaces = await _db.ClickUpSpaces
            .Include(x => x.Lists)
            .ToListAsync(cancellationToken);

        foreach (var space in spaces)
        {
            var listsResponse = await _api.Lists.GetNotInFolderAsync(space.Id);
            var lists = listsResponse.Lists.Select(x => new ClickUpListDbo
            {
                Id = x.Id,
                SpaceId = x.Space.Id,
                Name = x.Name,
            });

            space.Lists.SoftReplace(
                replacement: lists,
                match: (o, n) => o.Id == n.Id,
                add: n => n,
                update: (o, n) =>
                {
                    o.Name = n.Name;
                },
                delete: o => _db.Remove(o)
            );

            await _db.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        return Ok();
    }
}