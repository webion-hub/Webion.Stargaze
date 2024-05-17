namespace Webion.ClickUp.Api.V2.Common;

public sealed class Status11Dto
{
    public required string Status { get; init; }
    public string? Id { get; init; }
    public required string Color { get; init; }
    public string? Type { get; init; }
    public int? Index { get; init; }
    public bool? HideLabel { get; init; }
}