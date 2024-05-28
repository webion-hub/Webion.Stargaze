using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Sync.Synchronization;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Objects;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/objects")]
[Tags("ClickUp")]
[ApiVersion("1.0")]
public sealed class SyncClickUpObjectsController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClickUpApi _api;
    private readonly ClickUpSettings _settings;

    public SyncClickUpObjectsController(StargazeDbContext db, IClickUpApi api, IOptions<ClickUpSettings> settings)
    {
        _db = db;
        _api = api;
        _settings = settings.Value;
    }

    /// <summary>
    /// Synchronize objects
    /// </summary>
    /// <remarks>
    /// Synchronizes all spaces, folders and lists.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var sync = new ClickUpObjectsSynchronizer(_db, _api);
        await sync.SynchronizeAsync(_settings.TeamId, cancellationToken);
        return Ok();
    }
}