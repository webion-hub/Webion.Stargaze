using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Tasks.Update;

public sealed class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    private readonly StargazeDbContext _db;

    public UpdateTaskRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MustAsync(NotBeADuplicatedTitle)
            .WithMessage("Task title can not be duplicated");

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .MustAsync(BeAValidProjectId)
            .WithMessage("ProjectId does not exist");
    }

    private async Task<bool> NotBeADuplicatedTitle(string title, CancellationToken cancellationToken)
    {
        return !await _db.Tasks
            .AnyAsync(x => x.Title == title, cancellationToken);
    }

    private async Task<bool> BeAValidProjectId(Guid ProjectId, CancellationToken cancellationToken)
    {
        return await _db.Projects
            .AnyAsync(x => x.Id == ProjectId, cancellationToken);
    }
}