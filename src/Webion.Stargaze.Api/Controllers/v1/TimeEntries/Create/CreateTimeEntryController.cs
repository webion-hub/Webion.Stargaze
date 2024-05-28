using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/entries")]
[Tags("Time Entries")]
[ApiVersion("1.0")]
public class CreateTimeEntryController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateTimeEntryRequestValidator _requestValidator;

    public CreateTimeEntryController(StargazeDbContext db, CreateTimeEntryRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPost]
    [ProducesResponseType<CreateTimeEntryRequest>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTimeEntryRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var duration = request.End - request.Start;

        var entry = new TimeEntryDbo
        {
            UserId = request.UserId,
            TaskId = request.TaskId,
            Description = request.Description,
            Start = request.Start,
            End = request.End,
            Duration = duration,
            Billable = request.Billable
        };

        _db.TimeEntries.Add(entry);
        await _db.SaveChangesAsync(cancellationToken);

        return Created("v1/time/entry", new CreateTimeEntryResponse
        {
            Id = entry.Id,
        });
    }
}