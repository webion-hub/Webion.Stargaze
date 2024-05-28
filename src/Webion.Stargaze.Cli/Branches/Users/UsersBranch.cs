using Spectre.Console.Cli;
using Webion.Stargaze.Cli.Branches.Users.Password;

namespace Webion.Stargaze.Cli.Branches.Users;

public sealed class UsersBranch : IBranchConfig
{
    public IBranchConfigurator Configure(IConfigurator config)
    {
        return config.AddBranch("users", users =>
        {
            users.AddBranch("password", password =>
            {
                password.AddCommand<ResetPasswordCommand>("reset");
            });
        }).WithAlias("user");
    }
}