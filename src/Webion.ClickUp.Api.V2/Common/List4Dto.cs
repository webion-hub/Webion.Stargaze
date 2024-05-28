namespace Webion.ClickUp.Api.V2.Common;

public sealed class List4Dto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int OrderIndex { get; init; }
    public string? Content { get; init; }
    public required Status11Dto Status { get; init; }
    public required Priority4Dto Priority { get; init; }
    public string? Assegnee { get; init; }
    public required int TaskCount { get; init; }
    public required string DueDate { get; init; }
    public required string StartDate { get; init; }
    public Folder3Dto? Folder { get; init; }
    public required Space2Dto Space { get; init; }
    public required bool Archived { get; init; }
    public bool? OverrideStatuses { get; init; }
    public IEnumerable<StatusDto>? Statuses { get; init; }
    public required string PermissionLevel { get; init; }
}