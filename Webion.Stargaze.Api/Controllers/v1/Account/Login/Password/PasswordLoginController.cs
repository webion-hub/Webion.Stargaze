using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Services.Clients;
using Webion.Stargaze.Auth.Services.Jwt;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Account.Login.Password;

[AllowAnonymous]
[ApiController]
[Route("v{version:apiVersion}/account/login")]
[ApiVersion("1.0")]
[Tags("Account")]
public sealed class PasswordLoginController : ControllerBase
{
    private readonly IJwtIssuer _jwtIssuer;
    private readonly UserManager<UserDbo> _userManager;
    private readonly SignInManager<UserDbo> _signInManager;
    private readonly IClientsManager _clientsManager;

    public PasswordLoginController(SignInManager<UserDbo> signInManager, IJwtIssuer jwtIssuer, UserManager<UserDbo> userManager, IClientsManager clientsManager)
    {
        _signInManager = signInManager;
        _jwtIssuer = jwtIssuer;
        _userManager = userManager;
        _clientsManager = clientsManager;
    }

    /// <summary>
    /// Password login
    /// </summary>
    /// <remarks>
    /// Allows direct access with a user's userName and password.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType<PasswordLoginResponse>(200)]
    public async Task<IActionResult> Login(
        [FromBody] PasswordLoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var client = await _clientsManager.FindByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            return Forbid();

        var hasValidSecret = _clientsManager.VerifySecret(client, request.ClientSecret);
        if (!hasValidSecret)
            return Forbid();
        
        
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            ModelState.AddModelError(nameof(request.UserName), "User does not exist");
            return ValidationProblem();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user: user,
            password: request.Password,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
            return Forbid();

        var pair = await _jwtIssuer.IssuePairAsync(user, client, cancellationToken);
        return Ok(new PasswordLoginResponse
        {
            AccessToken = pair.AccessToken,
            RefreshToken = pair.RefreshToken,
        });
    }
}