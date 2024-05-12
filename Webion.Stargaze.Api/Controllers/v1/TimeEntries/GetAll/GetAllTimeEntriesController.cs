using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/time/entries")]
[Tags("Time Entries")]
public sealed class GetAllTimeEntriesController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllTimeEntriesController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllTimeEntriesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTimeEntriesRequest request,
        CancellationToken cancellationToken
    )
    {
        var page = await _db.TimeEntries
            .Where(x => x.Start >= request.From)
            .Where(x => x.End <= request.To)
            .If(request.ProjectId is not null, b => b
                .Where(x => x.Task!.ProjectId == request.ProjectId)
            )
            .If(request.TaskId is not null, b => b
                .Where(x => x.TaskId == request.TaskId)
            )
            .If(request.AssignedTo.Any(), b => b
                .Where(x => request.AssignedTo.Contains(x.UserId))
            )
            .Select(x => new TimeEntryDto
            {
                Id = x.Id,
                UserId = x.UserId,
                TaskId = x.TaskId,
                Description = x.Description,
                Start = x.Start,
                End = x.End,
                Duration = x.Duration,
                Locked = x.Locked,
                Billable = x.Billable,
                Billed = x.Billed,
                Paid = x.Paid,
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);
        
        return Ok(GetAllTimeEntriesResponse.From(page));
    }
}