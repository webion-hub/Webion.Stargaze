using Spectre.Console.Cli;

namespace Webion.Stargaze.Cli.Branches.Clients;

public sealed class ClientsBranch : IBranchConfig
{
    public IBranchConfigurator Configure(IConfigurator config)
    {
        return config.AddBranch("clients", clients =>
        {
            clients.AddCommand<CreateClientCommand>("create");
        }).WithAlias("client");
    }
}