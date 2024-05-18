using Spectre.Console;
using Spectre.Console.Cli;
using Webion.Stargaze.Auth.Services.Clients;
using Webion.Stargaze.Cli.Core;
using Webion.Stargaze.Cli.Ui;

namespace Webion.Stargaze.Cli.Branches.Clients;

public sealed class CreateClientCommand : AsyncCommand<CreateClientCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--name")]
        public string? Name { get; init; }
    }

    private readonly IClientsManager _clientsManager;
    private readonly ICliApplicationLifetime _lifetime;

    public CreateClientCommand(IClientsManager clientsManager, ICliApplicationLifetime lifetime)
    {
        _clientsManager = clientsManager;
        _lifetime = lifetime;
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var name = settings.Name ?? AnsiConsole.Ask<string>(Msg.Ask("Client name"));

        return await AnsiConsole.Status().StartAsync("Creating client...", async _ =>
        {
            var result = await _clientsManager.CreateAsync(
                name: name,
                cancellationToken: _lifetime.CancellationToken
            );

            switch (result)
            {
                case CreateClientResult.Duplicate:
                    AnsiConsole.MarkupLine(Msg.Err($"A client with name {name} already exists"));
                    return 1;
                
                case CreateClientResult.Created c:
                    AnsiConsole.MarkupLine(Msg.Ok($"Client {c.Client.Name} created"));
                    AnsiConsole.MarkupLine(Msg.Ok($"Client id: [yellow]{c.Client.Id}[/]"));
                    AnsiConsole.MarkupLine(Msg.Ok($"Client secret: [blue]{c.PlainTextSecret}[/]"));
                    return 0;
                
                default:
                    return -1;
            }
        });
    }
}