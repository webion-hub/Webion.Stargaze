using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class LinkedObjectDto
{
    public required string Id { get; init; }
    public required string? Name { get; init; }
    public required ClickUpObjectType Type { get; init; }
    public required string? Path { get; init; }
}