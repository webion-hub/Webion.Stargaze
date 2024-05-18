using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Services.Clients;
using Webion.Stargaze.Auth.Services.Jwt.Exchange;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Exchange;

[AllowAnonymous]
[ApiController]
[Route("v{version:apiVersion}/connect/token/exchange/{code}")]
[Tags("Connect")]
[ApiVersion("1.0")]
public sealed class ExchangeCodeController : ControllerBase
{
    private readonly IClientsManager _clientsManager;
    private readonly IExchangeCodeManager _exchangeCodeManager;

    public ExchangeCodeController(IExchangeCodeManager exchangeCodeManager, IClientsManager clientsManager)
    {
        _exchangeCodeManager = exchangeCodeManager;
        _clientsManager = clientsManager;
    }
    
    [HttpPost]
    [ProducesResponseType<ExchangeCodeResponse>(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Exchange(
        [FromRoute] Guid code,
        [FromBody] ExchangeCodeRequest request,
        CancellationToken cancellationToken
    )
    {
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