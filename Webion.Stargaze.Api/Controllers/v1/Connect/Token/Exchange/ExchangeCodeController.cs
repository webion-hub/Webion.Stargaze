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
    private readonly ILogger<ExchangeCodeController> _logger;

    public ExchangeCodeController(IExchangeCodeManager exchangeCodeManager, IClientsManager clientsManager, ILogger<ExchangeCodeController> logger)
    {
        _exchangeCodeManager = exchangeCodeManager;
        _clientsManager = clientsManager;
        _logger = logger;
    }

    /// <summary>
    /// Code exchange
    /// </summary>
    /// <remarks>
    /// Exchanges a code obtained from an external sign-in with a pair of access and refresh tokens.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<ExchangeCodeResponse>(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Exchange(
        [FromRoute] Guid code,
        [FromBody] ExchangeCodeRequest request,
        CancellationToken cancellationToken
    )
    {
        var client = await _clientsManager.FindByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
        {
            _logger.LogWarning("Could not find client with id {ClientId}", request.ClientId);
            return Forbid();
        }

        var isValid = _clientsManager.VerifySecret(client, request.ClientSecret);
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