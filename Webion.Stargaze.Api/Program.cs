using FluentValidation;
using Qubi.Api.Config;
using Webion.Application.Extensions;
using Webion.AspNetCore;
using Webion.Stargaze.Api;
using Webion.Stargaze.Api.Config;
using Webion.Stargaze.Api.Config.Swagger;
using Webion.Stargaze.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Add<LoggingConfig>();

builder.Add<ApiVersioningConfig>();
builder.Add<SwaggerConfig>();
builder.Add<OptionsConfig>();
builder.Add<ControllersConfig>();
builder.Add<ClickUpApiConfig>();

builder.Add<AuthNConfig>();
builder.Add<AuthZConfig>();

builder.Add<CorsConfig>();
builder.Add<StorageConfig>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddValidatorsFromAssemblyContaining<StargazeApiAssemblyMarker>();
builder.Services.AddModulesFromAssembly<StargazeAuthAssemblyMarker>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";
app.Urls.Add(url);

app.Use<CorsConfig>();
app.Use<AuthNConfig>();
app.Use<AuthZConfig>();

app.Use<SwaggerConfig>();
app.Use<ControllersConfig>();
app.Run();
