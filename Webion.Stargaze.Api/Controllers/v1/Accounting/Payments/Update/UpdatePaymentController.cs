using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Update;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/payments/{paymentId}")]
[Tags("Payments")]
[ApiVersion("1.0")]
public sealed class UpdatePaymentController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private UpdatePaymentRequestValidator _requestValidator;

    public UpdatePaymentController(StargazeDbContext db, UpdatePaymentRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid paymentId,
        [FromBody] UpdatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var updatedRows = await _db.Payments
            .Where(x => x.Id == paymentId)
            .ExecuteUpdateAsync(
                cancellationToken: cancellationToken,
                setPropertyCalls: b => b
                    .SetProperty(x => x.BankAccountId, request.BankAccountId)
                    .SetProperty(x => x.CategoryId, request.CategoryId)
                    .SetProperty(x => x.From, request.From)
                    .SetProperty(x => x.To, request.To)
                    .SetProperty(x => x.Description, request.Description)
                    .SetProperty(x => x.Amount, request.Amount)
                    .SetProperty(x => x.Type, request.Type)
                    .SetProperty(x => x.Status, request.Status)
                    .SetProperty(x => x.PaidAt, request.PaidAt)
            );

        if (updatedRows <= 0)
            return NotFound();

        return NoContent();
    }
}