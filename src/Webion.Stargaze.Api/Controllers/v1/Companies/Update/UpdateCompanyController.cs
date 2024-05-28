using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies/{companyId}")]
[Tags("Companies")]
[ApiVersion("1.0")]
public sealed class UpdateCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly UpdateCompanyRequestValidator _requestValidator;

    public UpdateCompanyController(StargazeDbContext db, UpdateCompanyRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Update company
    /// </summary>
    /// <remarks>
    /// Updates a company.
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

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