using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects")]
[Tags("Projects")]
[ApiVersion("1.0")]
public sealed class CreateProjectController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateProjectRequestValidator _requestValidator;

    public CreateProjectController(StargazeDbContext db, CreateProjectRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Create project
    /// </summary>
    /// <remarks>
    /// Creates a project.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<CreateProjectResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

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