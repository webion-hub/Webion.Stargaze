using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Link.ClickUp;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/projects/{projectId:guid}/link")]
[ApiVersion("1.0")]
[Tags("Projects")]
public class GetAllLinkedObjectsController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllLinkedObjectsController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllLinkedObjectsResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var linkedLists = await _db.Projects
            .Where(x => x.Id == projectId)
            .SelectMany(x => x.ClickUpLists)
            .Select(x => new LinkedObjectDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        var linkedSpaces = await _db.Projects
            .Where(x => x.Id == projectId)
            .SelectMany(x => x.ClickUpSpaces)
            .Select(x => new LinkedObjectDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        var linkedFolders = await _db.Projects
            .Where(x => x.Id == projectId)
            .SelectMany(x => x.ClickUpFolders)
            .Select(x => new LinkedObjectDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        var response = new GetAllLinkedObjectsResponse
        {
            LinkedObjects = linkedLists.Concat(linkedSpaces).Concat(linkedFolders)
        };

        return Ok(response);
    }
}