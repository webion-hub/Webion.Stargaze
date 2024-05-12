using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Core;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies")]
[Tags("Companies")]
public sealed class CreateCompanyController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public CreateCompanyController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [ProducesResponseType<CreateCompanyResponse>(201)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyRequest request,
        CancellationToken cancellationToken
    )
    {
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