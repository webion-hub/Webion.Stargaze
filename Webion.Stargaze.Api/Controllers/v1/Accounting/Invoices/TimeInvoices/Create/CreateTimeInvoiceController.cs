using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Webion.Stargaze.Api.Extensions;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Accounting;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.TimeInvoices.Create;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/invoices/time-invoices")]
[Tags("Invoices")]
[ApiVersion("1.0")]
public sealed class CreateTimeInvoiceController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly CreateInvoiceRequestValidator _requestValidator;

    public CreateTimeInvoiceController(StargazeDbContext db, CreateInvoiceRequestValidator requestValidator)
    {
        _db = db;
        _requestValidator = requestValidator;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTimeInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        await _requestValidator.ValidateModelAsync(request, ModelState, cancellationToken);
        if (!ModelState.IsValid)
            return ValidationProblem();

        var tasksIds = await _db.TimePackages
            .Where(x => x.Id == request.TimePackageId)
            .SelectMany(x => x.Company.Projects)
            .SelectMany(x => x.Tasks)
            .AsNoTracking()
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var timeEntries = await _db.TimeEntries
            .Where(x => x.Billable)
            .Where(x => tasksIds.Contains(x.TaskId ?? Guid.Empty))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (timeEntries.IsNullOrEmpty())
            return Problem(
                detail: "The time package has not time entries linked",
                statusCode: StatusCodes.Status400BadRequest
            ); ;

        var timeInvoice = new TimeInvoiceDbo
        {
            InvoiceId = request.InvoiceId,
            TimePackageId = request.TimePackageId,
            InvoicedTime = request.InvoicedTime
        };
        _db.TimeInvoices.Add(timeInvoice);

        var invoicedTime = timeInvoice.InvoicedTime;

        foreach (var timeEntry in timeEntries)
        {
            invoicedTime -= timeEntry.Duration;

            var timeEntryInvoice = new TimeEntryInvoiceDbo
            {
                TimeInvoiceId = timeInvoice.Id,
                TimeEntryId = timeEntry.Id,
                BilledTime = invoicedTime > TimeSpan.Zero
                    ? timeEntry.Duration
                    : -invoicedTime
            };
            _db.TimeEntryInvoices.Add(timeEntryInvoice);
        }

        await _db.SaveChangesAsync(cancellationToken);
        return Created();
    }
}