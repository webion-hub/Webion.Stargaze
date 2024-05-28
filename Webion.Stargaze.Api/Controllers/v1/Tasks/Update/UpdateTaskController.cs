using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/tasks/{taskId}")]
[Tags("Tasks")]
[ApiVersion("1.0")]
public sealed class UpdateTaskController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdateTaskRequestValidator _requestValidator;

    public UpdateTaskController(StargazeDbContext db, UpdateTaskRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Update task
    /// </summary>
    /// <remarks>
    /// Updates a task.
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid taskId,
        [FromBody] UpdateTaskRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var updatedRows = await _db.Tasks
            .Where(x => x.Id == taskId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.ProjectId, request.ProjectId)
                    .SetProperty(x => x.Title, request.Title)
                    .SetProperty(x => x.Description, request.Description)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}