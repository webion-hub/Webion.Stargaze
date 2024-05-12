using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId}")]
[Tags("Projects")]
public sealed class UpdateProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public UpdateProjectController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var updatedRows = await _db.Projects
            .Where(x => x.Id == projectId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.CompanyId, request.CompanyId)
                    .SetProperty(x => x.Name, request.Name)
                    .SetProperty(x => x.Description, request.Description)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}