using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/{invoiceId}")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class UpdateInvoiceController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdateInvoiceRequestValidator _requestValidator;

    public UpdateInvoiceController(StargazeDbContext db, UpdateInvoiceRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Update invoice
    /// </summary>
    /// <remarks>
    /// Updates an invoice.
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid invoiceId,
        [FromBody] UpdateInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var updatedRows = await _db.Invoices
            .Where(x => x.Id == invoiceId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.IssuedById, request.IssuedById)
                    .SetProperty(x => x.IssuedToId, request.IssuedToId)
                    .SetProperty(x => x.NetPrice, request.NetPrice)
                    .SetProperty(x => x.TaxedPrice,
                        request.NetPrice * (request.VatPercentage / 100 + 1))
                    .SetProperty(x => x.VatPercentage, request.VatPercentage)
                    .SetProperty(x => x.Paid, request.Paid)
                    .SetProperty(x => x.Type, request.Type)
                    .SetProperty(x => x.EmittedAt, request.EmittedAt)
                    .SetProperty(x => x.ExpiresAt, request.ExpiresAt)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}