using Refit;
using Webion.ClickUp.Api.Team.Dtos;

namespace Webion.ClickUp.Api.Team;

public interface IClickUpTeamApi
{
    /// <summary>
    /// View the Workspaces available to the authenticated user.
    /// </summary>
    /// <returns></returns>
    [Get("/v2/team")]
    Task<GetTeamsResponse> GetAllAsync();
    
    /// <summary>
    /// View time entries filtered by start and end date.
    /// </summary>
    /// <remarks>
    /// By default, this endpoint returns time entries from the last 30 days created by the authenticated user.
    /// <br/>
    /// To retrieve time entries for other users, you must include the assignee query parameter.
    /// <br/>
    /// Only one of the following location filters can be included at a time: space_id, folder_id, list_id, or task_id.
    /// <br/><br/>
    /// Note: A time entry that has a negative duration means that timer is currently running for that user.
    /// </remarks>
    [Get("/v2/team/{teamId}/time_entries")]
    Task<GetTeamTimeEntriesResponse> GetTimeEntriesAsync(long teamId, [Query] GetTeamTimeEntriesRequest request);
}