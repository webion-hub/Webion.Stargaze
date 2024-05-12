using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/tasks")]
[Tags("Tags")]
public sealed class GetAllTasksController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllTasksController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllTasksResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTasksRequest request,
        CancellationToken cancellationToken
    )
    {
        var tasks = await _db.Tasks
            .If(request.CompanyId is not null, b => b
                .Where(x => x.Project.CompanyId == request.CompanyId)
            )
            .If(request.ProjectId is not null, b => b
                .Where(x => x.ProjectId == request.ProjectId)
            )
            // .If(request.AssignedTo.Any(), b => b
            //     .Where(x => x.)
            // )
            .Select(x => new TaskDto
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Title = x.Title,
                Description = x.Description,
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetAllTasksResponse.From(tasks));
    }
}