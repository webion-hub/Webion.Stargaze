using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Core;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies")]
[Tags("Companies")]
[ApiVersion("1.0")]
public sealed class CreateCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateCompanyRequestValidator _requestValidator;

    public CreateCompanyController(StargazeDbContext db, CreateCompanyRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPost]
    [ProducesResponseType<CreateCompanyResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();
        
        var company = new CompanyDbo
        {
            Name = request.Name,
        };

        _db.Companies.Add(company);
        await _db.SaveChangesAsync(cancellationToken);
        
        return Created($"v1/companies/{company.Id}", new CreateCompanyResponse
        {
            Id = company.Id,
        });
    }
}