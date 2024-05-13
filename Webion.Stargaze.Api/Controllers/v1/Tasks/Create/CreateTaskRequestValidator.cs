using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Create;

public sealed class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    private readonly StargazeDbContext _db;

    public CreateTaskRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.Title).NotEmpty().MustAsync(NotBeADuplicateTitle);
    }

    private async Task<bool> NotBeADuplicateTitle(string title, CancellationToken cancellationToken)
    {
        return !await _db.Tasks
            .Where(x => x.Title == title)
            .AnyAsync(cancellationToken);
    }
}