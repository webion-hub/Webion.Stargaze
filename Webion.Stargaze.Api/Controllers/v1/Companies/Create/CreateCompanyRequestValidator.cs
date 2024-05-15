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
        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(NotBeADuplicatedName)
            .WithMessage("Company name can not be duplicated");
    }

    private async Task<bool> NotBeADuplicatedName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Companies
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}