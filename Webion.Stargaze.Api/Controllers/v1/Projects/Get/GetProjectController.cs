using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId}")]
[Tags("Project")]
public sealed class GetProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetProjectController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetProjectResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var response = await _db.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new GetProjectResponse
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Name = x.Name,
                Description = x.Description,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}