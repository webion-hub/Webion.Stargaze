using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Time.Current.GetAll;

public sealed class GetAllClickUpCurrentTimeResponse
{
    public required IEnumerable<ClickUpCurrentTimeEntryDto> TimeEntries { get; init; }
}