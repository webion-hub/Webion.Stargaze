using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.OAuth.Dtos;

public sealed class GetAuthorizedUserResponse
{
    public required ClickUpUserDto User { get; init; }
}