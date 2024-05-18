using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Services.Jwt;
using Webion.Stargaze.Auth.Services.Jwt.Exchange;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Account.Login.External;

[AllowAnonymous]
[ApiController]
[Route("v{version:apiVersion}/account/login/external/handle")]
[Tags("Account")]
[ApiVersion("1.0")]
public sealed class EndExternalLoginController : ControllerBase
{
    private readonly UserManager<UserDbo> _userManager;
    private readonly IJwtIssuer _jwtIssuer;
    private readonly StargazeDbContext _db;
    private readonly SignInManager<UserDbo> _signInManager;
    private readonly IExchangeCodeManager _exchangeCodeManager;
    private readonly ILogger<EndExternalLoginController> _logger;

    public EndExternalLoginController(UserManager<UserDbo> userManager, IExchangeCodeManager exchangeCodeManager, IJwtIssuer jwtIssuer, StargazeDbContext db, ILogger<EndExternalLoginController> logger, SignInManager<UserDbo> signInManager)
    {
        _userManager = userManager;
        _exchangeCodeManager = exchangeCodeManager;
        _jwtIssuer = jwtIssuer;
        _db = db;
        _logger = logger;
        _signInManager = signInManager;
    }


    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Callback(
        [FromQuery] ExternalLoginRequest request,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            return Problem("User information not found");

        var user = await _userManager.FindByLoginAsync(
            loginProvider: info.LoginProvider,
            providerKey: info.ProviderKey
        );


        user ??= await ImportExternalUserAsync(info);
        if (user is null || !ModelState.IsValid)
            return ValidationProblem();

        var client = await _db.Clients.FindAsync([request.ClientId], cancellationToken);
        if (client is null)
        {
            _logger.LogWarning("Could not find client with id {ClientId}", request.ClientId);
            return Forbid();
        }
        
        var pair = await _jwtIssuer.IssuePairAsync(user, client, cancellationToken);
        var code = await _exchangeCodeManager.GetCodeAsync(pair);

        await transaction.CommitAsync(cancellationToken);
        return Redirect(
            $"{request.ExchangeUri}" +
            $"?code={code}" +
            $"&redirectUri={request.RedirectUri}"
        );
    }

    private async Task<UserDbo?> ImportExternalUserAsync(ExternalLoginInfo info)
    {
        var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (userName is null)
        {
            _logger.LogWarning("ClaimTypes.Name was null");
            return null;
        }

        var user = new UserDbo
        {
            UserName = userName.Replace(" ", ".").Trim(),
            Email = email,
        };

        var res = await _userManager.CreateAsync(user);
        if (!res.Succeeded)
        {
            foreach (var e in res.Errors)
                ModelState.AddModelError(e.Code, e.Description);

            return null;
        }

        var addLoginRes = await _userManager.AddLoginAsync(user, new UserLoginInfo(
            loginProvider: info.LoginProvider,
            providerKey: info.ProviderKey,
            displayName: info.ProviderDisplayName
        ));

        if (!addLoginRes.Succeeded)
        {
            foreach (var e in addLoginRes.Errors)
                ModelState.AddModelError(e.Code, e.Description);

            return null;
        }

        return user;
    }
}