using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Accounting;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Items.Add;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}/items")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class AddInvoiceItemController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public AddInvoiceItemController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Add invoice item
    /// </summary>
    /// <remarks>
    /// Adds an item to an invoice.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<AddInvoiceItemResponse>(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid invoiceId,
        [FromBody] AddInvoiceItemRequest request,
        CancellationToken cancellationToken
    )
    {
        var invoiceExist = await _db.Invoices
            .AnyAsync(x => x.Id == invoiceId, cancellationToken);

        if (!invoiceExist)
            return NotFound();

        var invoiceItem = new InvoiceItemDbo
        {
            InvoiceId = invoiceId,
            TotalUnits = request.TotalUnits,
            Description = request.Description,
            NetPrice = request.NetPrice,
            TaxedPrice = request.NetPrice * (request.VatPercentage / 100 + 1),
            VatPercentage = request.VatPercentage,
        };

        _db.InvoiceItems.Add(invoiceItem);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/invoices/{invoiceId}/items/{invoiceItem.Id}", new AddInvoiceItemResponse
        {
            Id = invoiceItem.Id,
        });
    }
}