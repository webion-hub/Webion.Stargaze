using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class DeleteInvoiceController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeleteInvoiceController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Delete invoice
    /// </summary>
    /// <remarks>
    /// Deletes an invoice.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.Invoices
            .Where(x => x.Id == invoiceId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}