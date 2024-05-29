using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.User.Timesheet.Get;

public sealed class GetUserTimesheetResponse
{
    public required IEnumerable<TimeEntryDto> TimeEntries { get; init; }
}