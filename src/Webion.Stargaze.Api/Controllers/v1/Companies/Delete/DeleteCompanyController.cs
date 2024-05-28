using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies/{companyId:guid}")]
[Tags("Companies")]
[ApiVersion("1.0")]
public sealed class DeleteCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteCompanyController(StargazeDbContext db)
    {
        _db = db;
    }


    /// <summary>
    /// Delete company
    /// </summary>
    /// <remarks>
    /// Deletes a company.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.Companies
            .Where(x => x.Id == companyId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}