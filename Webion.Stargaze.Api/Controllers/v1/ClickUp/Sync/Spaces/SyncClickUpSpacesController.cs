using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.Spaces.Dtos;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.ClickUp;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Spaces;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/spaces")]
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]
public sealed class SyncClickUpSpacesController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;
    private readonly ClickUpSettings _settings;

    public SyncClickUpSpacesController(StargazeDbContext db, IClickUpApi api, IOptions<ClickUpSettings> settings)
    {
        _db = db;
        _api = api;
        _settings = settings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var spacesResponse = await _api.Spaces.GetAllAsync(
            teamId: _settings.TeamId,
            request: new GetAllSpacesRequest()
        );
        
        _db.ClickUpSpaces.AddRange(spacesResponse.Spaces.Select(x => new ClickUpSpaceDbo
        {
            Id = x.Id,
            Name = x.Name,
        }));

        await _db.SaveChangesAsync(cancellationToken);
        return Ok(spacesResponse.Spaces);
    }
}