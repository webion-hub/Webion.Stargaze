using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Users.GetAll;

public sealed class GetAllUsersResponse
{
    public required IEnumerable<UserDto> Users { get; init; }
}