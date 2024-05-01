using Webion.ClickUp.Api;
using Webion.ClickUp.Api.Team.Dtos;
using Webion.Stargaze.Api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<ClickUpSettings>()
    .BindConfiguration(ClickUpSettings.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var settings = builder.Configuration
    .GetSection(ClickUpSettings.Section)
    .Get<ClickUpSettings>()!;

builder.Services.AddClickUpApi(settings.ApiKey);

var app = builder.Build();

app.MapGet("/", async (ClickUpApi api) =>
{
    return Results.Ok(
        await api.Teams.GetTimeEntriesAsync(settings.TeamId, new GetTeamTimeEntriesRequest
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(-2).ToUnixTimeMilliseconds(),
            EndDate = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(),
        })
    );
});

app.Run();
