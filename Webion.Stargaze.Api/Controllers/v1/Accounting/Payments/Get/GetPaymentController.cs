using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/payments/{paymentId}")]
[Tags("Payments")]
[ApiVersion("1.0")]
public sealed class GetPaymentController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetPaymentController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType<GetPaymentResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken
    )
    {
        var response = await _db.Payments
            .Where(x => x.Id == paymentId)
            .Select(x => new GetPaymentResponse
            {
                Id = x.Id,
                InvoiceId = x.InvoiceId,
                BankAccountId = x.BankAccountId,
                CategoryId = x.CategoryId,
                From = x.From,
                To = x.To,
                Description = x.Description,
                Amount = x.Amount,
                Type = x.Type,
                Status = x.Status
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}