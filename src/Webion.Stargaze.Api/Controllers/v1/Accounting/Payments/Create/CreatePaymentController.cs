using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Accounting;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/payments")]
[Tags("Payments")]
[ApiVersion("1.0")]
public sealed class CreatePaymentController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreatePaymentRequestValidator _requestValidator;

    public CreatePaymentController(StargazeDbContext db, CreatePaymentRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Create payment
    /// </summary>
    /// <remarks>
    /// Creates a payment.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<CreatePaymentResponse>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var payment = new PaymentDbo
        {
            BankAccountId = request.BankAccountId,
            CategoryId = request.CategoryId,
            From = request.From,
            To = request.To,
            Description = request.Description,
            Amount = request.Amount,
            Type = request.Type,
            Status = request.Status,
            PaidAt = request.PaidAt,
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(cancellationToken);

        return Created($"v1/payments/{payment.Id}", new CreatePaymentResponse
        {
            Id = payment.Id,
        });
    }
}