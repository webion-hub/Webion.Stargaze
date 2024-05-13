using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Create;

public sealed class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    private readonly StargazeDbContext _db;

    public CreateCompanyRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.Name).NotEmpty().MustAsync(NotBeADuplicateName);
    }

    private async Task<bool> NotBeADuplicateName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Companies
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }
}