using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Create;

public sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    private readonly StargazeDbContext _db;

    public CreateProjectRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(NotBeADuplicatedName)
            .WithMessage("Project name can not be duplicated");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .MustAsync(BeAValidCompanyId)
            .WithMessage("CompanyId does not exist");
    }

    private async Task<bool> NotBeADuplicatedName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Projects
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }

    private async Task<bool> BeAValidCompanyId(Guid CompanyId, CancellationToken cancellationToken)
    {
        return await _db.Companies
            .Where(x => x.Id == CompanyId)
            .AnyAsync(cancellationToken);
    }
}