using Webion.ClickUp.Api;
using Webion.ClickUp.Api.Team.Dtos;
using Webion.Stargaze.Api.Converters;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;
using Webion.Stargaze.Pgsql.Extensions;

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

builder.Services.ConfigureHttpJsonOptions(x =>
{
    x.SerializerOptions.Converters.Add(new TypeIdConverter());
});

var conn = builder.Configuration.GetConnectionString("db")!;
builder.Services.AddStargazeDbContext(conn);

var app = builder.Build();

app.MapGet("/", async (ClickUpApi api, StargazeDbContext db) =>
{
    var entriesResponse = await api.Teams.GetTimeEntriesAsync(settings.TeamId, new GetTeamTimeEntriesRequest
    {
        StartDate = DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeMilliseconds(),
        EndDate = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(),
    });

    var users = entriesResponse.Data
        .DistinctBy(x => x.User.Id)
        .ToDictionary(x => x.User.Id, x => x.User);
    
    var newUsers = entriesResponse.Data
        .GroupBy(x => x.User.Id)
        .Select(g => new UserDbo
        {
            Id = IEntityBase.New<UserDbo>(),
            UserName = users[g.Key].Username,
            NormalizedUserName = users[g.Key].Username.ToUpper(),
            Email = users[g.Key].Email,
            NormalizedEmail = users[g.Key].Email.ToUpper(),
            EmailConfirmed = true,
            FirstName = "",
            LastName = "",
            Enabled = true,
            TimeEntries = g
                .Select(x => new TimeEntryDbo
                {
                    Id = IEntityBase.New<TimeEntryDbo>(),
                    Start = x.Start,
                    End = x.End,
                    Duration = x.Duration,
                })
                .ToList()
        })
        .ToList();
    
    db.AddRange(newUsers);
    await db.SaveChangesAsync();
    
    return Results.Ok(newUsers
        .SelectMany(x => x.TimeEntries)
        .Select(x => new
        {
            x.User.UserName,
            x.User.Email,
            x.Start,
            x.End,
            x.Duration,
        })
    );
});

app.Run();
