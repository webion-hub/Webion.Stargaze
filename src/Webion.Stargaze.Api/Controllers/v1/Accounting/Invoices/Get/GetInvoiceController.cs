using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class GetInvoiceController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetInvoiceController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetInvoiceResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var response = await _db.Invoices
            .Where(x => x.Id == invoiceId)
            .Select(x => new GetInvoiceResponse
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
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}