using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using Webion.Application.Extensions;
using Webion.Stargaze.Auth;
using Webion.Stargaze.Cli.Branches;
using Webion.Stargaze.Cli.Core;
using Webion.Stargaze.Cli.DI;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Cli;

public static class CliAppBuilder
{
    public static CommandApp Build(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(config);

        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

        services.AddLogging(o =>
        {
            o.AddConsole();
            o.SetMinimumLevel(LogLevel.Warning);
        });

        services.AddModulesFromAssembly<StargazeAuthAssemblyMarker>();
        services.AddStargazeDbContext(config.GetConnectionString("db")!);
        services.AddMemoryCache();

        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);

        app.Configure(o =>
        {
            o.AddBranchesFromAssemblyContaining<StargazeCliAssemblyMarker>();
        });

        return app;
    }
}