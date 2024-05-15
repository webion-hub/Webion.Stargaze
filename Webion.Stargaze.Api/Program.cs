using FluentValidation;
using Qubi.Api.Config;
using Qubi.Api.Config.Swagger;
using Webion.Application.Extensions;
using Webion.AspNetCore;
using Webion.Stargaze.Api;
using Webion.Stargaze.Api.Config;
using Webion.Stargaze.Auth;

var builder = WebApplication.CreateBuilder(args);

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

app.Use<SwaggerConfig>();

app.Use<CorsConfig>();
app.Use<AuthNConfig>();
app.Use<AuthZConfig>();

app.Use<ControllersConfig>();
app.Run("http://localhost:5000");
