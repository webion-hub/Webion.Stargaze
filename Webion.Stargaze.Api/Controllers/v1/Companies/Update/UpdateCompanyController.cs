using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies/{companyId}")]
[Tags("Companies")]
public sealed class UpdateCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public UpdateCompanyController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken
    )
    {
        var updatedRows = await _db.Companies
            .Where(x => x.Id == companyId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.Name, request.Name)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}