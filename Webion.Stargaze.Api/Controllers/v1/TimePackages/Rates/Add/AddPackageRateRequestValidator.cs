using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Rates.Add;

public sealed class AddPackageRateRequestValidator : AbstractValidator<AddPackageRateRequest>
{
    private readonly StargazeDbContext _db;

    public AddPackageRateRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(BeAValidUserId)
            .WithMessage("UserId does not exist");
    }

    private async Task<bool> BeAValidUserId(Guid userId, CancellationToken cancellationToken)
    {
        return await _db.Users
            .Where(x => x.Id == userId)
            .AnyAsync(cancellationToken);
    }
}