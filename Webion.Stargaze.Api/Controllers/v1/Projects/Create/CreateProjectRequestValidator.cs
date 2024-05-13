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
        RuleFor(x => x.Name).NotEmpty().MustAsync(NotBeADuplicateName);
    }

    private async Task<bool> NotBeADuplicateName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Projects
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }
}