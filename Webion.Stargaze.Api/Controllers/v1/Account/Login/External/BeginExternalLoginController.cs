using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Account.Login.External;

[AllowAnonymous]
[ApiController]
[Route("v{version:apiVersion}/account/login/external/{provider}")]
[Tags("Account")]
[ApiVersion("1.0")]
public sealed class BeginExternalLoginController : ControllerBase
{
    private readonly SignInManager<UserDbo> _signInManager;

    public BeginExternalLoginController(SignInManager<UserDbo> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(
        [FromRoute] LoginProvider provider,
        [FromQuery] ExternalLoginRequest request
    )
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            provider: provider.ToString(),
            redirectUrl: Url.Action("Callback", controller: "EndExternalLogin", new
            {
                request.ClientId,
                request.ExchangeUri,
                request.RedirectUri,
            })
        );

        return Challenge(properties, provider.ToString());
    }
}