using Webion.ClickUp.Api.Common;

namespace Webion.ClickUp.Api.OAuth.Dtos;

public sealed class GetAuthorizedUserResponse
{
    public required UserDto User { get; init; }
}