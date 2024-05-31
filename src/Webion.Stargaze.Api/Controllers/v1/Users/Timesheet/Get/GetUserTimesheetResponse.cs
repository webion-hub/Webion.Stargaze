using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Users.Timesheet.Get;

public sealed class GetUserTimesheetResponse : PaginatedResponse<TimeEntryDto, GetUserTimesheetResponse>
{
    public IEnumerable<TimeEntryDto> TimeEntries { get; init; } = [];
}