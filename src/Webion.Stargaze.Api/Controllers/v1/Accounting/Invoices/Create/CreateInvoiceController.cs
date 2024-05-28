using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Accounting;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class CreateInvoiceController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateInvoiceRequestValidator _requestValidator;

    public CreateInvoiceController(StargazeDbContext db, CreateInvoiceRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Create invoice
    /// </summary>
    /// <remarks>
    /// Creates an invoice.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<CreateInvoiceResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var invoice = new InvoiceDbo
        {
            IssuedById = request.IssuedById,
            IssuedToId = request.IssuedToId,
            NetPrice = request.NetPrice,
            TaxedPrice = request.NetPrice * (request.VatPercentage / 100 + 1),
            VatPercentage = request.VatPercentage,
            Paid = request.Paid,
            Type = request.Type,
            EmittedAt = request.EmittedAt,
            ExpiresAt = request.ExpiresAt,
        };

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/invoices/{invoice.Id}", new CreateInvoiceResponse
        {
            Id = invoice.Id,
        });
    }
}