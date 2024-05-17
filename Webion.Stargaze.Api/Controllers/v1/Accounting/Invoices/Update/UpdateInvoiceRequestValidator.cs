using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Update;

public sealed class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequest>
{
    private readonly StargazeDbContext _db;

    public UpdateInvoiceRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.IssuedById)
            .MustAsync(BeAValidCompanyId)
            .WithMessage("IssuedById does not exist");

        RuleFor(x => x.IssuedToId)
            .MustAsync(BeAValidCompanyId)
            .WithMessage("IssuedToId does not exist");

        RuleFor(x => new TimeRange(x.EmittedAt, x.ExpiresAt))
            .Must(BeAValidTimeRange)
            .WithMessage("Time range is invalid: EmittedAt is higher than ExpiresAt");

        RuleFor(x => x.EmittedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("EmittedAt cannot be set in the future");

        RuleFor(x => x.ExpiresAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("ExpiresAt cannot be set in the future");
    }

    private async Task<bool> BeAValidCompanyId(Guid? CompanyId, CancellationToken cancellationToken)
    {
        if (CompanyId is null)
            return true;

        return await _db.Companies
            .AnyAsync(x => x.Id == CompanyId, cancellationToken);
    }

    private static bool BeAValidTimeRange(TimeRange range)
    {
        return range.From <= range.To;
    }

    private record TimeRange(DateTimeOffset From, DateTimeOffset To);
}