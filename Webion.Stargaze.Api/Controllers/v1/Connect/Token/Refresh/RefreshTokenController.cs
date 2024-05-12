using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Refresh;

[Authorize]
[ApiController]
[Route("v1/connect/token/refresh")]
[Tags("Connect")]
public sealed class RefreshTokenController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public RefreshTokenController(StargazeDbContext db)
    {
        _db = db;
    }
}