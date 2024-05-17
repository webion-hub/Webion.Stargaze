using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Update;

public sealed class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
{
    private readonly StargazeDbContext _db;

    public UpdatePaymentRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.BankAccountId)
            .MustAsync(BeAValidBankAccountId)
            .WithMessage("BankAccountId does not exist");

        RuleFor(x => x.CategoryId)
            .MustAsync(BeAValidCategoryId)
            .WithMessage("CategoryId does not exist");

        RuleFor(x => x.PaidAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("PaidAt cannot be set in the future");
    }

    private async Task<bool> BeAValidBankAccountId(Guid? BankAccountId, CancellationToken cancellationToken)
    {
        if (BankAccountId is null)
            return true;

        return await _db.BankAccounts
            .AnyAsync(x => x.Id == BankAccountId, cancellationToken);
    }

    private async Task<bool> BeAValidCategoryId(Guid? CategoryId, CancellationToken cancellationToken)
    {
        if (CategoryId is null)
            return true;

        return await _db.PaymentCategories
            .AnyAsync(x => x.Id == CategoryId, cancellationToken);
    }
}