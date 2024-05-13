using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Update;

public sealed class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public Guid CompanyId { get; set; }
    
    private readonly StargazeDbContext _db;

    public UpdateCompanyRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.Name).NotEmpty().MustAsync(NotBeADuplicateName);
    }

    private async Task<bool> NotBeADuplicateName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Companies
            .Where(x => x.Id != CompanyId)
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }
}