using Webion.Stargaze.Cli;

var app = CliAppBuilder.Build(args);

return await app.RunAsync(args);