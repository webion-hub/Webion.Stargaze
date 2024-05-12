using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Auth.Services.Jwt;
using Webion.Stargaze.Auth.Services.Jwt.Exchange;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Login.External;

[AllowAnonymous]
[ApiController]
[Route("v1/account/login/external/handle")]
public sealed class EndExternalLoginController : ControllerBase
{
    private readonly UserManager<UserDbo> _userManager;
    private readonly IJwtIssuer _jwtIssuer;
    private readonly StargazeDbContext _db;
    private readonly SignInManager<UserDbo> _signIn;
    private readonly IExchangeCodeManager _exchangeCodeManager;
    private readonly ILogger<EndExternalLoginController> _logger;
    
    public EndExternalLoginController(UserManager<UserDbo> userManager, IExchangeCodeManager exchangeCodeManager, IJwtIssuer jwtIssuer, StargazeDbContext db, ILogger<EndExternalLoginController> logger, SignInManager<UserDbo> signIn)
    {
        _userManager = userManager;
        _exchangeCodeManager = exchangeCodeManager;
        _jwtIssuer = jwtIssuer;
        _db = db;
        _logger = logger;
        _signIn = signIn;
    }

    
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Callback(
        [FromQuery] ExternalLoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var info = await _signIn.GetExternalLoginInfoAsync();
        if (info is null)
            return Problem("User information not found");
        
        var user = await _userManager.FindByLoginAsync(
            loginProvider: info.LoginProvider,
            providerKey: info.ProviderKey
        );

        
        user ??= await ImportClickUpUserAsync(info);
        if (user is null || !ModelState.IsValid)
            return ValidationProblem();
        

        var client = await _db.Clients.FindAsync([request.ClientId], cancellationToken);
        if (client is null)
        {
            _logger.LogWarning("Could not find client with id {ClientId}", request.ClientId);
            return BadRequest();
        }
        
        var pair = await _jwtIssuer.IssuePairAsync(user, client);
        var code = await _exchangeCodeManager.GetCodeAsync(pair);
        
        return Redirect(
            $"{request.ExchangeUri}" +
            $"?code={code}" +
            $"&redirectUri={request.RedirectUri}"
        );
    }

    private async Task<UserDbo?> ImportClickUpUserAsync(ExternalLoginInfo info)
    {
        var user = new UserDbo
        {
            UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
        };
            
        var res = await _userManager.CreateAsync(user);
        if (!res.Succeeded)
        {
            foreach (var e in res.Errors)
                ModelState.AddModelError(e.Code, e.Description);

            return null;
        }
            
        var loginRes = await _userManager.AddLoginAsync(user, new UserLoginInfo(
            loginProvider: LoginProviders.ClickUp,
            providerKey: info.ProviderKey,
            displayName: LoginProviders.ClickUp
        ));

        if (!loginRes.Succeeded)
        {
            foreach (var e in loginRes.Errors)
                ModelState.AddModelError(e.Code, e.Description);

            return null;
        }

        return user;
    }
}