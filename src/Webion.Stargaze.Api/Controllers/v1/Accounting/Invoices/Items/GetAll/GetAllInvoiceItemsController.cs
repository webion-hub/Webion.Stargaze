using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Items.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}/items")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class GetAllInvoiceItemsController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllInvoiceItemsController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all invoice items
    /// </summary>
    /// <remarks>
    /// Returns all invoice items.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllInvoiceItemsResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var invoices = await _db.InvoiceItems
            .Where(x => x.InvoiceId == invoiceId)
            .Select(x => new GetAllInvoiceItemsResponse
            {
                Id = x.Id,
                InvoiceId = x.InvoiceId,
                TotalUnits = x.TotalUnits,
                Description = x.Description,
                NetPrice = x.NetPrice,
                TaxedPrice = x.TaxedPrice,
                VatPercentage = x.VatPercentage
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (invoices is null)
            return NotFound();

        return Ok(invoices);
    }
}