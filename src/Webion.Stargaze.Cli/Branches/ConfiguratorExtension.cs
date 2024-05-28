using System.Reflection;
using Spectre.Console.Cli;

namespace Webion.Stargaze.Cli.Branches;

public static class ConfiguratorExtension
{
    public static IBranchConfigurator AddBranch<TConfig>(this IConfigurator config)
        where TConfig : IBranchConfig, new()
    {
        return new TConfig().Configure(config);
    }

    public static void AddBranchesFromAssemblyContaining<T>(this IConfigurator config)
    {
        var assembly = typeof(T).Assembly;
        var types = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => t
                .GetInterfaces()
                .Any(x => x == typeof(IBranchConfig))
            );
        
        foreach (var type in types)
        {
            var addBranchMethod = typeof(ConfiguratorExtension).GetMethod(
                name: nameof(AddBranch),
                bindingAttr: BindingFlags.Static | BindingFlags.Public
            );
                
            var concreteMethod = addBranchMethod?.MakeGenericMethod(type);
            concreteMethod?.Invoke(null, [config]);
        }
    }
}