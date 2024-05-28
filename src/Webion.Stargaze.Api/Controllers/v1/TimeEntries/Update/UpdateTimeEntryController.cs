using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/entries/{timeEntryId}")]
[Tags("Time Entries")]
[ApiVersion("1.0")]
public sealed class UpdateTimeEntryController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdateTimeEntryRequestValidator _requestValidator;

    public UpdateTimeEntryController(StargazeDbContext db, UpdateTimeEntryRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid timeEntryId,
        [FromBody] UpdateTimeEntryRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var updatedRows = await _db.TimeEntries
            .Where(x => x.Id == timeEntryId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.UserId, request.UserId)
                    .SetProperty(x => x.Start, request.Start)
                    .SetProperty(x => x.Billable, request.Billable)
                    .SetProperty(x => x.TaskId, request.TaskId)
                    .SetProperty(x => x.Description, request.Description)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}