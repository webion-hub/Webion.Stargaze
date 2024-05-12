using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects")]
[Tags("Projects")]
public sealed class GetAllProjectsController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllProjectsController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllProjectsResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllProjectsRequest request,
        CancellationToken cancellationToken
    )
    {
        var projects = await _db.Projects
            .If(request.CompanyId is not null, b => b
                .Where(x => x.CompanyId == request.CompanyId)
            )
            .Select(x => new ProjectDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Name = x.Name,
                Description = x.Description,
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetAllProjectsResponse.From(projects));
    }
}