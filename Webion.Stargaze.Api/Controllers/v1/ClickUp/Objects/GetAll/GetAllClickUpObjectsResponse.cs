using Webion.Stargaze.Core.Entities;
using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Objects.GetAll;

public sealed class GetAllClickUpObjectsResponse
{
    public required IEnumerable<ClickUpObjectDto> Objects { get; init; }
}

public sealed class ClickUpObjectDto
{
    public required ClickUpObjectId Id { get; init; }
    public required ClickUpObjectType Type { get; init; }
    public required string? Path { get; init; }
}