using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies/{companyId}")]
[Tags("Companies")]
[ApiVersion("1.0")]
public sealed class GetCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetCompanyController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get company
    /// </summary>
    /// <remarks>
    /// Returns a company.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetCompanyResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken
    )
    {
        var response = await _db.Companies
            .Where(x => x.Id == companyId)
            .Select(x => new GetCompanyResponse
            {
                Id = x.Id,
                Name = x.Name,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}