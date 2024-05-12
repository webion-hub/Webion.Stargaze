using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects")]
[Tags("Projects")]
public sealed class CreateProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public CreateProjectController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [ProducesResponseType<CreateProjectResponse>(201)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var project = new ProjectDbo
        {
            CompanyId = request.CompanyId,
            Name = request.Name,
            Description = request.Description,
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/projects/{project.Id}", new CreateProjectResponse
        {
            Id = project.Id,
        });
    }
}