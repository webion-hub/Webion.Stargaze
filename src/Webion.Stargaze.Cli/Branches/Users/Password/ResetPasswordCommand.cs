using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Spectre.Console;
using Spectre.Console.Cli;
using Webion.Stargaze.Cli.Ui;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Cli.Branches.Users.Password;

public sealed class ResetPasswordCommand : AsyncCommand<ResetPasswordCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--user-name")]
        public string? UserName { get; init; }
        
        [CommandOption("--new-password-length")]
        public int? NewPasswordLength { get; init; }
    }

    private readonly UserManager<UserDbo> _userManager;

    public ResetPasswordCommand(UserManager<UserDbo> userManager)
    {
        _userManager = userManager;
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var userName = settings.UserName ?? AnsiConsole.Ask<string>(Msg.Ask("User name"));
        var newPasswordLength = settings.NewPasswordLength ?? AnsiConsole.Ask(Msg.Ask("New password length"), 12);
        
        var bytes = RandomNumberGenerator.GetBytes(newPasswordLength);
        var newPassword = Convert.ToBase64String(bytes)[..newPasswordLength];

        return await AnsiConsole.Status().StartAsync("Searching user", async ctx =>
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                AnsiConsole.MarkupLine(Msg.Err("User does not exist"));
                return 1;
            }

            AnsiConsole.MarkupLine(Msg.Ok("User found"));
            ctx.Status("Removing password");
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                AnsiConsole.Write(IdentityErrorsTable.From(removeResult.Errors));
                return 2;
            }

            AnsiConsole.MarkupLine(Msg.Ok("Password removed"));
            ctx.Status("Adding password");
            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                AnsiConsole.Write(IdentityErrorsTable.From(addResult.Errors));
                return 3;
            }

            AnsiConsole.MarkupLine(Msg.Ok($"Added new password [yellow]{newPassword}[/]"));
            return 0;
        });
    }
}