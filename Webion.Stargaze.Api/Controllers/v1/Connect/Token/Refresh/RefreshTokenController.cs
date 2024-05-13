using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Refresh;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/connect/token/refresh")]
[Tags("Connect")]
[ApiVersion("1.0")]
public sealed class RefreshTokenController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public RefreshTokenController(StargazeDbContext db)
    {
        _db = db;
    }
}