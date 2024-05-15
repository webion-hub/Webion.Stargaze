namespace Webion.ClickUp.Api.V2.Common;

public sealed class List4
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int OrderIndex { get; init; }
    public string? Content { get; init; }
    public required Status11 Status { get; init; }
    public required Priority4 Priority { get; init; }
    public string? Assegnee { get; init; }
    public required int TaskCount { get; init; }
    public required string DueDate { get; init; }
    public required string StartDate { get; init; }
    public required Folder3 Folder { get; init; }
    public required Space2 Space { get; init; }
    public required bool Archived { get; init; }
    public bool? OverrideStatuses { get; init; }
    public required string PermissionLevel { get; init; }
}