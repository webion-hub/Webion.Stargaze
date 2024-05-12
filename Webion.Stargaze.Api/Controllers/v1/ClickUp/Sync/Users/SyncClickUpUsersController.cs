using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.ClickUp.Api.V2;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Users;

[ApiController]
[Route("v1/clickup/sync/users")]
[Tags("ClickUp Sync")]
public sealed class SyncClickUpUsersController : ControllerBase
{
    private readonly IClickUpApi _clickUpApi;
    private readonly UserManager<UserDbo> _userManager;
    private readonly ClickUpSettings _clickUpSettings;
    private readonly StargazeDbContext _db;

    public SyncClickUpUsersController(IClickUpApi clickUpApi, IOptions<ClickUpSettings> clickUpSettings, StargazeDbContext db, UserManager<UserDbo> userManager)
    {
        _clickUpApi = clickUpApi;
        _db = db;
        _clickUpSettings = clickUpSettings.Value;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        
        var teamsResponse = await _clickUpApi.Teams.GetAllAsync();
        var team = teamsResponse.Teams.First(x => x.Id == _clickUpSettings.TeamId);
        
        foreach (var m in team.Members)
        {
            var exists = await _userManager.FindByLoginAsync(
                providerKey: m.User.Id,
                loginProvider: ClickUpDefaults.AuthenticationScheme
            );

            if (exists is not null)
                continue;

            var user = new UserDbo
            {
                UserName = m.User.UserName
                    .Replace(" ", ".")
                    .ToLower(),
            };
            
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                foreach (var e in createResult.Errors)
                    ModelState.AddModelError(e.Code, e.Description);
                
                continue;
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(
                loginProvider: ClickUpDefaults.AuthenticationScheme,
                providerKey: m.User.Id,
                displayName: ClickUpDefaults.DisplayName
            ));
            
            foreach (var e in addLoginResult.Errors)
                ModelState.AddModelError(e.Code, e.Description);
        }

        if (!ModelState.IsValid)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ValidationProblem();
        }

        await transaction.CommitAsync(cancellationToken);

        var users = await _db.Users
            .Select(x => new
            {
                x.Id,
                x.UserName,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return Ok(users);
    }
}