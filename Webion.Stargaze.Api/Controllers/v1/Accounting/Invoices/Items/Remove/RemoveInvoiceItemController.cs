using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Items.Remove;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}/items/{invoiceItemId}")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class RemoveInvoiceItemController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public RemoveInvoiceItemController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid invoiceId,
        [FromRoute] Guid invoiceItemId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.InvoiceItems
            .Where(x => x.InvoiceId == invoiceId)
            .Where(x => x.Id == invoiceItemId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}