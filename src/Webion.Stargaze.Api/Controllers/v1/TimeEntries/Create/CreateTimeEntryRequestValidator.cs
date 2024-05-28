using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.TimeEntries.Create;

public sealed class CreateTimeEntryRequestValidator : AbstractValidator<CreateTimeEntryRequest>
{
    private readonly StargazeDbContext _db;

    public CreateTimeEntryRequestValidator(StargazeDbContext db)
    {
        _db = db;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(BeAValidUserId)
            .WithMessage("UserId does not exist");

        RuleFor(x => x.TaskId)
            .MustAsync(BeAValidTaskId)
            .WithMessage("TaskId does not exist");

        RuleFor(x => new TimeRange(x.Start, x.End))
            .Must(BeAValidTimeRange)
            .WithMessage("Time range is invalid: Start is higher than End");

        RuleFor(x => x.Start)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start cannot be set in the future");

        RuleFor(x => x.End)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("End cannot be set in the future");
    }

    private async Task<bool> BeAValidUserId(Guid UserId, CancellationToken cancellationToken)
    {
        return await _db.Users
            .AnyAsync(x => x.Id == UserId, cancellationToken);
    }

    private async Task<bool> BeAValidTaskId(Guid? TaskId, CancellationToken cancellationToken)
    {
        if (TaskId is null)
            return true;

        return await _db.Tasks
            .AnyAsync(x => x.Id == TaskId, cancellationToken);
    }

    private static bool BeAValidTimeRange(TimeRange range)
    {
        return range.From <= range.To;
    }

    private record TimeRange(DateTimeOffset From, DateTimeOffset To);
}