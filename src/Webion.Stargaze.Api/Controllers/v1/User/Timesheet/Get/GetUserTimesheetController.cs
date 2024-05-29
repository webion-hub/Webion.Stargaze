using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Extensions.EntityFrameworkCore;
using Webion.Stargaze.Api.Controllers.Dtos;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Controllers.v1.User.Timesheet.Get;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/user/timesheet")]
[Tags("User")]
[ApiVersion("1.0")]
public sealed class GetUserTimesheetController : ControllerBase
{
    private readonly StargazeDbContext _db;
    private readonly SignInManager<UserDbo> _signInManager;
    private readonly UserManager<UserDbo> _userManager;

    public GetUserTimesheetController(StargazeDbContext db, SignInManager<UserDbo> signInManager, UserManager<UserDbo> userManager)
    {
        _db = db;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Get user timesheet
    /// </summary>
    /// <remarks>
    /// Returns the timesheet of the logged user.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<GetUserTimesheetResponse>(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get(
        [FromQuery] GetUserTimesheetRequest request,
        CancellationToken cancellationToken
    )
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            return Problem("User information not found");

        var user = await _userManager.FindByLoginAsync(
            loginProvider: info.LoginProvider,
            providerKey: info.ProviderKey
        );

        var page = await _db.TimeEntries
            .Where(x => x.UserId == user!.Id)
            .Select(x => new TimeEntryDto
            {
                Id = x.Id,
                UserId = x.UserId,
                TaskId = x.TaskId,
                Description = x.Description,
                Start = x.Start,
                End = x.End,
                Duration = x.Duration,
                Locked = x.Locked,
                Billable = x.Billable,
                Billed = x.Billed,
                Paid = x.Paid,
            })
            .AsNoTracking()
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(GetUserTimesheetResponse.From(page));
    }
}