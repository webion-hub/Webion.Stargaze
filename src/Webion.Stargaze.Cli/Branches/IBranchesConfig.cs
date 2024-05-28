using Spectre.Console.Cli;

namespace Webion.Stargaze.Cli.Branches;

public interface IBranchConfig
{
    IBranchConfigurator Configure(IConfigurator config);
}