using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Services.Link;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Link.ClickUp;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId:guid}/link/")]
[ApiVersion("1.0")]
[Tags("Projects")]
public class LinkClickUpObjectController : ControllerBase
{
    private readonly ClickUpLinkerService _linker;
    public LinkClickUpObjectController(ClickUpLinkerService linker)
    {
        _linker = linker;
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
        [FromBody] LinkClickUpObjectRequest request
    )
    {
        var result = await _linker.LinkAsync(projectId, request.ClickUpObjectIds);

        if (!result)
            NotFound();

        return Ok();
    }
}