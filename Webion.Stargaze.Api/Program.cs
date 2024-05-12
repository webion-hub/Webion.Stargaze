using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Webion.Application.Extensions;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.ClickUp.Api;
using Webion.ClickUp.Api.V2;
using Webion.Stargaze.Api;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Auth;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<ClickUpSettings>()
    .BindConfiguration(ClickUpSettings.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddIdentity<UserDbo, RoleDbo>()
    .AddEntityFrameworkStores<StargazeDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddTransient<ClickUpApiAuthHandler>();
builder.Services
    .AddClickUpApi()
    // .AddHttpMessageHandler<ClickUpApiAuthHandler>();
    .ConfigureHttpClient(x =>
    {
        x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            builder.Configuration["ClickUp:ApiKey"]!
        );
    });

builder.Services.AddHttpContextAccessor();


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddGoogle(options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    })
    .AddClickUp(options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = builder.Configuration["ClickUp:ClientId"]!;
        options.ClientSecret = builder.Configuration["ClickUp:ClientSecret"]!;
    });

builder.Services.AddAuthorization();


builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(x =>
    {
        x.AllowAnyHeader();
        x.AllowAnyMethod();
        x.AllowCredentials();
        x.SetIsOriginAllowed(_ => true);
    });
});

builder.Services.ConfigureHttpJsonOptions(x =>
{
    x.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddMemoryCache();
builder.Services.AddModulesFromAssembly<StargazeAuthAssemblyMarker>();

var conn = builder.Configuration.GetConnectionString("db")!;
builder.Services.AddStargazeDbContext(conn);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
