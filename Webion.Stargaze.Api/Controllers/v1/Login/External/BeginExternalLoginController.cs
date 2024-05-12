using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Login.External;

[AllowAnonymous]
[ApiController]
[Route("v1/account/login/external/{provider}")]
public sealed class BeginExternalLoginController : ControllerBase
{
    private readonly SignInManager<UserDbo> _signInManager;

    public BeginExternalLoginController(SignInManager<UserDbo> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(
        [FromRoute]
        [AllowedValues(LoginProviders.ClickUp)]
        string provider,
        
        [FromQuery]
        ExternalLoginRequest request
    )
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            provider: provider,
            redirectUrl: Url.Action("Callback", controller: "EndExternalLogin", new
            {
                request.ClientId,
                request.ExchangeUri,
                request.RedirectUri,
            })
        );
        
        return Challenge(properties, provider);
    }
}