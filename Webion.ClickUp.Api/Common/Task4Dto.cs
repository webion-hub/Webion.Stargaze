namespace Webion.ClickUp.Api.Common;

public sealed class Task4Dto
{
    public string? Id { get; init; }
    public string? CustomId { get; init; }
    public string? Name { get; init; }
    public StatusDto? Status { get; init; }
    public string? CustomType { get; init; }
}