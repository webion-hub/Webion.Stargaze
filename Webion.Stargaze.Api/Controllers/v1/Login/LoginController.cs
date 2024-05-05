using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Webion.ClickUp.Api;
using Webion.ClickUp.Api.OAuth.Dtos;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql.Entities;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.Login;

[ApiController]
[Route("v1/account/login")]
public sealed class LoginController : ControllerBase
{
    private readonly ClickUpSettings _clickUpSettings;
    private readonly IClickUpApi _clickUpApi;
    private readonly UserManager<UserDbo> _userManager;
    private readonly SignInManager<UserDbo> _signInManager;

    public LoginController(IOptions<ClickUpSettings> clickUpSettings, IClickUpApi clickUpApi, UserManager<UserDbo> userManager, SignInManager<UserDbo> signInManager)
    {
        _clickUpApi = clickUpApi;
        _userManager = userManager;
        _signInManager = signInManager;
        _clickUpSettings = clickUpSettings.Value;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return Redirect(
            $"https://app.clickup.com/api" +
            $"?client_id={_clickUpSettings.ClientId}" +
            $"&redirect_uri=http://localhost:5000/v1/account/login/callback"
        );
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string? code)
    {
        if (code is null)
            return Redirect("https://google.com");
        
        var tok = await _clickUpApi.OAuth.GetAccessTokenAsync(new GetAccessTokenRequest
        {
            ClientId = _clickUpSettings.ClientId,
            ClientSecret = _clickUpSettings.ClientSecret,
            Code = code,
        });
        
        var userInfo = await _clickUpApi.OAuth.GetUserAsync(tok.AccessToken);
        var user = await _userManager.FindByLoginAsync(
            loginProvider: "ClickUp",
            providerKey: userInfo.User.Id.ToString()
        );

        if (user is null)
        {
            user = new UserDbo
            {
                UserName = userInfo.User.UserName
                    .Replace(" ", ".")
                    .ToLower(),
            };
            
            var res = await _userManager.CreateAsync(user);
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors)
                    ModelState.AddModelError(e.Code, e.Description);

                return ValidationProblem();
            }
            
            var loginRes = await _userManager.AddLoginAsync(user, new ExternalLoginInfo(
                principal: User,
                loginProvider: "ClickUp",
                providerKey: userInfo.User.Id.ToString(),
                displayName: "ClickUp"
            ));

            if (!loginRes.Succeeded)
            {
                foreach (var e in loginRes.Errors)
                    ModelState.AddModelError(e.Code, e.Description);

                return ValidationProblem();
            }
        }

        await _signInManager.SignInWithClaimsAsync(
            user: user,
            isPersistent: false,
            additionalClaims:
            [
                new Claim("clickup_access_token", tok.AccessToken),
            ]
        );
        
        return Ok();
    }
}