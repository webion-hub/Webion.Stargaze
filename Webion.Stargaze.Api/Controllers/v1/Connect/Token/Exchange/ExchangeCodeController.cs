using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Auth.Services.Clients;
using Webion.Stargaze.Auth.Services.Jwt.Exchange;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Exchange;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/connect/token/exchange/{code}")]
[Tags("Connect")]
[ApiVersion("1.0")]
public sealed class ExchangeCodeController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IClientsManager _clientsManager;
    private readonly IExchangeCodeManager _exchangeCodeManager;
    private readonly ILogger<ExchangeCodeController> _logger;

    public ExchangeCodeController(StargazeDbContext db, IExchangeCodeManager exchangeCodeManager, ILogger<ExchangeCodeController> logger, IClientsManager clientsManager)
    {
        _db = db;
        _exchangeCodeManager = exchangeCodeManager;
        _logger = logger;
        _clientsManager = clientsManager;
    }
    
    [HttpPost]
    [ProducesResponseType<ExchangeCodeResponse>(200)]
    public async Task<IActionResult> Exchange(
        [FromRoute] Guid code,
        [FromBody] ExchangeCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        var client = await _db.Clients
            .Where(x => x.Id == request.ClientId)
            .FirstOrDefaultAsync(cancellationToken);

        var isValid = await _clientsManager.VerifyAsync(
            id: request.ClientId,
            base64Secret: request.ClientSecret,
            cancellationToken: cancellationToken
        );

        if (!isValid)
            return Forbid();
        
        var tokenPair = await _exchangeCodeManager.ExchangeCodeAsync(code);
        if (tokenPair is null)
            return Forbid();

        return Ok(new ExchangeCodeResponse
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken,
        });
    }
}