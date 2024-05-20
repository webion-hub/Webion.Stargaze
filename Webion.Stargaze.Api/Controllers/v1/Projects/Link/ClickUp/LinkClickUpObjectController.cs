using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Core.Entities;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Link.ClickUp;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId:guid}/link/{clickUpObjectId}")]
[ApiVersion("1.0")]
[Tags("Projects")]
public class LinkClickUpObjectController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public LinkClickUpObjectController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Link clickup object
    /// </summary>
    /// <remarks>
    /// Links a clickup object to this project.<br/>
    /// This will ensure that all synchronized tasks will be added to it.
    /// </remarks>
    [HttpPut]
    public async Task<IActionResult> Link(
        [FromRoute] Guid projectId,
        [FromRoute] ClickUpObjectId clickUpObjectId
    )
    {
        
        
        return Ok();
    }
}