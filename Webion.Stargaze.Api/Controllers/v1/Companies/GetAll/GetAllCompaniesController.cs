using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/companies")]
[Tags("Companies")]
[ApiVersion("1.0")]
public sealed class GetAllCompaniesController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllCompaniesController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all companies
    /// </summary>
    /// <remarks>
    /// Returns all companies.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllCompaniesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllCompaniesRequest request,
        CancellationToken cancellationToken
    )
    {
        var page = await _db.Companies
            .Select(x => new CompanyDto
            {
                Id = x.Id,
                Name = x.Name,
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetAllCompaniesResponse.From(page));
    }
}