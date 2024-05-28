using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/payments")]
[Tags("Payments")]
[ApiVersion("1.0")]
public sealed class GetAllPaymentsController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllPaymentsController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all payments
    /// </summary>
    /// <remarks>
    /// Returns all payments filtered by invoice, bank account or category.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllPaymentsResponse>(200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllPaymentsRequest request,
        CancellationToken cancellationToken
    )
    {
        var payments = await _db.Payments
            .If(request.InvoiceId is not null, b => b
                .Where(x => x.InvoiceId == request.InvoiceId)
            )
            .If(request.BankAccountId is not null, b => b
                .Where(x => x.BankAccountId == request.BankAccountId)
            )
            .If(request.CategoryId is not null, b => b
                .Where(x => x.CategoryId == request.CategoryId)
            )
            .Select(x => new PaymentDto
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
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetAllPaymentsResponse.From(payments));
    }
}