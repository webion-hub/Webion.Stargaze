using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.User.Timesheet.Get;

public sealed class GetUserTimesheetResponse : PaginatedResponse<TimeEntryDto, GetUserTimesheetResponse>
{
    public IEnumerable<TimeEntryDto> TimeEntries { get; init; } = [];
}