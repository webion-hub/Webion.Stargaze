using Refit;
using Webion.ClickUp.Api.Common;

namespace Webion.ClickUp.Api.Team.Dtos;

public class GetTeamTimeEntriesRequest
{
    /// <summary>
    /// Unix time in milliseconds
    /// </summary>
    [AliasAs("start_date")]
    public long? StartDate { get; init; }
    
    /// <summary>
    /// Unix time in milliseconds
    /// </summary>
    [AliasAs("end_date")]
    public long? EndDate { get; init; }
    
    /// <summary>
    /// Filter by user_id. For multiple assignees, separate user_id using commas.
    /// 
    /// Example: assignee=1234,9876
    /// Note: Only Workspace Owners/Admins have access to do this.
    /// </summary>
    [AliasAs("assignee")]
    public IEnumerable<ClickUpId> Assignee { get; init; } = [];
    
    /// <summary>
    /// Include task tags in the response for time entries associated with tasks.
    /// </summary>
    [AliasAs("include_task_tags")]
    public bool? IncludeTaskTags { get; init; }
    
    /// <summary>
    /// Include the names of the List, Folder, and Space along with the list_id, folder_id, and space_id.
    /// </summary>
    [AliasAs("include_location_names")]
    public bool? IncludeLocationNames { get; init; }
    
    /// <summary>
    /// Only include time entries associated with tasks in a specific Space.
    /// </summary>
    [AliasAs("space_id")]
    public long? SpaceId { get; init; }
    
    /// <summary>
    /// Only include time entries associated with tasks in a specific Folder.
    /// </summary>
    [AliasAs("folder_id")]
    public long? FolderId { get; init; }
    
    /// <summary>
    /// Only include time entries associated with tasks in a specific List.
    /// </summary>
    [AliasAs("list_id")]
    public long? ListId { get; init; }
    
    /// <summary>
    /// Only include time entries associated with a specific task.
    /// </summary>
    [AliasAs("task_id")]
    public string? TaskId { get; init; }

    /// <summary>
    /// If you want to reference a task by it's custom task id, this value must be true.
    /// </summary>
    [AliasAs("custom_task_ids")]
    public string[] CustomTaskIds { get; init; } = [];
    
    /// <summary>
    /// Only used when the custom_task_ids parameter is set to true.
    /// </summary>
    /// <example>
    /// For example: <c>custom_task_ids=true&amp;team_id=123.</c>
    /// </example>
    [AliasAs("team_id")]
    public ClickUpId? TeamId { get; init; }
    
    /// <summary>
    /// Include only billable time entries by using a value of true or only non-billable
    /// time entries by using a value of false.
    ///
    /// For example: ?is_billable=true.
    /// </summary>
    [AliasAs("is_billable")]
    public bool? IsBillable { get; init; }
}