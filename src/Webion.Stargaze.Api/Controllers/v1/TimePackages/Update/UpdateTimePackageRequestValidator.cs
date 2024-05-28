using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Update;

public sealed class UpdateTimePackageRequestValidator : AbstractValidator<UpdateTimePackageRequest>
{
    private readonly StargazeDbContext _db;

    public UpdateTimePackageRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .MustAsync(BeAValidCompanyId)
            .WithMessage("CompanyId does not exist");
    }

    private async Task<bool> BeAValidCompanyId(Guid CompanyId, CancellationToken cancellationToken)
    {
        return await _db.Companies
            .AnyAsync(x => x.Id == CompanyId, cancellationToken);
    }
}