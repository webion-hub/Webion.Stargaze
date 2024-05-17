using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class GetAllInvoicesController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllInvoicesController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetAllInvoicesResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllInvoicesRequest request,
        CancellationToken cancellationToken
    )
    {
        var invoices = await _db.Invoices
            .If(request.IssuedById is not null, b => b
                .Where(x => x.IssuedById == request.IssuedById)
            )
            .If(request.IssuedToId is not null, b => b
                .Where(x => x.IssuedToId == request.IssuedToId)
            )
            .Select(x => new InvoiceDto
            {
                Id = x.Id,
                IssuedById = x.IssuedById,
                IssuedToId = x.IssuedToId,
                NetPrice = x.NetPrice,
                TaxedPrice = x.TaxedPrice,
                VatPercentage = x.VatPercentage,
                Paid = x.Paid,
                Type = x.Type,
                EmittedAt = x.EmittedAt,
                ExpiresAt = x.ExpiresAt
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetAllInvoicesResponse.From(invoices));
    }
}
