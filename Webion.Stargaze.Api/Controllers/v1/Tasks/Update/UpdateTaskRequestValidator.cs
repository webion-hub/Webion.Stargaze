using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Update;

public sealed class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public Guid TaskId { get; set; }

    private readonly StargazeDbContext _db;

    public UpdateTaskRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.Title).NotEmpty().MustAsync(NotBeADuplicateTitle);
    }

    private async Task<bool> NotBeADuplicateTitle(string title, CancellationToken cancellationToken)
    {
        return !await _db.Tasks
            .Where(x => x.Id != TaskId)
            .Where(x => x.Title == title)
            .AnyAsync(cancellationToken);
    }
}