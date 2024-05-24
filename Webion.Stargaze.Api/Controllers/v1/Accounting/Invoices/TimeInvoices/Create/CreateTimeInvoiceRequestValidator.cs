using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.TimeInvoices.Create;

public sealed class CreateInvoiceRequestValidator : AbstractValidator<CreateTimeInvoiceRequest>
{
    private readonly StargazeDbContext _db;

    public CreateInvoiceRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.InvoiceId)
            .MustAsync(BeAValidInvoiceId)
            .WithMessage("InvoiceId does not exist");

        RuleFor(x => x.TimePackageId)
            .MustAsync(BeAValidTimePackageId)
            .WithMessage("TimePackageId does not exist");
    }

    private async Task<bool> BeAValidInvoiceId(Guid InvoiceId, CancellationToken cancellationToken)
    {
        return await _db.Invoices
            .AnyAsync(x => x.Id == InvoiceId, cancellationToken);
    }

    private async Task<bool> BeAValidTimePackageId(Guid TimePackageId, CancellationToken cancellationToken)
    {
        return await _db.TimePackages
            .AnyAsync(x => x.Id == TimePackageId, cancellationToken);
    }
}