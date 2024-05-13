using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Update;

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public Guid ProjectId { get; set; }

    private readonly StargazeDbContext _db;

    public UpdateProjectRequestValidator(StargazeDbContext db)
    {
        _db = db;
        RuleFor(x => x.Name).NotEmpty().MustAsync(NotBeADuplicateName);
    }

    private async Task<bool> NotBeADuplicateName(string name, CancellationToken cancellationToken)
    {
        return !await _db.Projects
            .Where(x => x.Id != ProjectId)
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }
}