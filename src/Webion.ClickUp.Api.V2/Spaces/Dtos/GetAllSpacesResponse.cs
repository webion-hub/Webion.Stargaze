using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Spaces.Dtos;

public sealed class GetAllSpacesResponse
{
    public required IEnumerable<Space13> Spaces { get; init; } = [];
}

public sealed class Space13
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public bool Private { get; init; }
    public string? Color { get; init; }
    public string? Avatar { get; init; }
    public bool? AdminCanManage { get; init; }
    public bool? Archived { get; init; }
    public IEnumerable<MemberDto> Members { get; init; } = [];
    public IEnumerable<StatusDto> Statuses { get; init; } = [];
    public bool MultipleAssignees { get; init; }
}