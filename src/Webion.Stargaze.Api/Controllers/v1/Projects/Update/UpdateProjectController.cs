using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId}")]
[Tags("Projects")]
[ApiVersion("1.0")]
public sealed class UpdateProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdateProjectRequestValidator _requestValidator;

    public UpdateProjectController(StargazeDbContext db, UpdateProjectRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

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