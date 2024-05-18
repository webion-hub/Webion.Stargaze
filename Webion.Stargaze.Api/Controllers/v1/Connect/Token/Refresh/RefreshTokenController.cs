using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Services.Clients;
using Webion.Stargaze.Auth.Services.Jwt;
using Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Connect.Token.Refresh;

[AllowAnonymous]
[ApiController]
[Route("v{version:apiVersion}/connect/token/refresh")]
[Tags("Connect")]
[ApiVersion("1.0")]
public sealed class RefreshTokenController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly IJwtIssuer _jwtIssuer;
    private readonly IClientsManager _clientsManager;
    private readonly TimeProvider _timeProvider;
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly ILogger<RefreshTokenController> _logger;

    public RefreshTokenController(StargazeDbContext db, TimeProvider timeProvider, IJwtIssuer jwtIssuer, IClientsManager clientsManager, ILogger<RefreshTokenController> logger, IRefreshTokenManager refreshTokenManager)
    {
        _db = db;
        _timeProvider = timeProvider;
        _jwtIssuer = jwtIssuer;
        _clientsManager = clientsManager;
        _logger = logger;
        _refreshTokenManager = refreshTokenManager;
    }
    
    [HttpPost]
    [ProducesResponseType<RefreshTokenResponse>(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> RefreshAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        
        var token = await _refreshTokenManager.RetrieveAsync(request.RefreshToken, cancellationToken);
        if (token is null)
            return Forbid();
        
        var client = await _clientsManager.FindByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
        {
            _logger.LogWarning("Could not find client with id {ClientId}", request.ClientId);
            return Forbid();
        }

        var isValid = _clientsManager.VerifySecret(client, request.ClientSecret);
        if (!isValid)
            return Forbid();
        
        var pair = await _jwtIssuer.IssuePairAsync(token.User, client, cancellationToken);
        token.ExpiresAt = _timeProvider.GetUtcNow();
        
        await _db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return Ok(new RefreshTokenResponse
        {
            AccessToken = pair.AccessToken,
            RefreshToken = pair.RefreshToken
        });
    }
}