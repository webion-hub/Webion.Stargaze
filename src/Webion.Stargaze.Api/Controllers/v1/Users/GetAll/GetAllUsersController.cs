using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.Linq;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Users.GetAll;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/users")]
[Tags("Users")]
[ApiVersion("1.0")]
public sealed class GetAllAppUsersController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public GetAllAppUsersController(StargazeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <remarks>
    /// Returns all the users.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetAllUsersResponse>(200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllUsersRequest request,
        CancellationToken cancellationToken
    )
    {
        var users = await _db.Users
            .If(request.Search is not null, x => x
                .Where(u =>
                    EF.Functions.TrigramsStrictWordSimilarityDistance(u.UserName!, request.Search!) < 0.85
                    || EF.Functions.TrigramsStrictWordSimilarityDistance(u.Email!, request.Search!) < 0.85
                )
            )
            .Select(u => new UserDto
            {
                UserId = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                PhoneNumber = u.PhoneNumber!,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(new GetAllUsersResponse
        {
            Users = users
        });
    }
}