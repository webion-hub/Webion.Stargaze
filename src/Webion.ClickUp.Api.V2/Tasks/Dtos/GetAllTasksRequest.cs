using Refit;

namespace Webion.ClickUp.Api.V2.Tasks.Dtos;

public sealed class GetAllTasksRequest
{
    [AliasAs("archived")]
    public bool? Archived { get; init; }

    [AliasAs("include_markdown_description")]
    public bool? IncludeMarkdownDescription { get; init; }

    [AliasAs("page")]
    public int? Page { get; init; }

    [AliasAs("order_by")]
    public string? OrderBy { get; init; }

    [AliasAs("reverse")]
    public bool? Reverse { get; init; }

    [AliasAs("subtasks")]
    public bool? Subtasks { get; init; }

    [AliasAs("statuses")]
    public IEnumerable<string>? Statuses { get; init; }

    [AliasAs("include_closed")]
    public bool? IncludeCloses { get; init; }

    [AliasAs("assignees")]
    public IEnumerable<string>? Assignees { get; init; }

    [AliasAs("watchers")]
    public IEnumerable<string>? Watchers { get; init; }

    [AliasAs("tags")]
    public IEnumerable<string>? Tags { get; init; }

    [AliasAs("due_date_gt")]
    public int? DueDateGt { get; init; }

    [AliasAs("due_date_lt")]
    public int? DueDateLt { get; init; }

    [AliasAs("date_created_gt")]
    public int? DateCreatedGt { get; init; }

    [AliasAs("date_created_lt")]
    public int? DateCreatedLt { get; init; }

    [AliasAs("date_updated_gt")]
    public int? DateUpdatedGt { get; init; }

    [AliasAs("date_updated_lt")]
    public int? DateUpdatedLt { get; init; }

    [AliasAs("date_done_gt")]
    public int? DateDoneGt { get; init; }

    [AliasAs("date_done_lt")]
    public int? DateDoneLt { get; init; }

    [AliasAs("custom_fields")]
    public IEnumerable<string>? CustomFields { get; init; }

    [AliasAs("custom_field")]
    public IEnumerable<string>? CustomField { get; init; }

    [AliasAs("custom_items")]
    public IEnumerable<int>? CustomItems { get; init; }
}