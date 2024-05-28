using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/tasks")]
[Tags("Tasks")]
[ApiVersion("1.0")]
public sealed class CreateTaskController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateTaskRequestValidator _requestValidator;

    public CreateTaskController(StargazeDbContext db, CreateTaskRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Create task
    /// </summary>
    /// <remarks>
    /// Creates a task.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<CreateTaskResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var task = new TaskDbo
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/tasks/{task.Id}", new CreateTaskResponse
        {
            Id = task.Id,
        });
    }
}