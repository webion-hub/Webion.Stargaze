using Microsoft.AspNetCore.Mvc;
using Spectre.Console;
using Webion.ClickUp.Api.V2;
using Webion.ClickUp.Api.V2.OAuth.Dtos;

const string clientId = "";
const string clientSecret = "";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClickUpApi();

var app = builder.Build();

app.MapGet("/", async (
    [FromQuery] string code,
    [FromServices] IClickUpApi api
) =>
{
    var tok = await api.OAuth.GetAccessTokenAsync(new GetAccessTokenRequest
    {
        ClientId = clientId,
        ClientSecret = clientSecret,
        Code = code,
    });

    
});

AnsiConsole.MarkupLine($"[blue]https://app.clickup.com/api?client_id={clientId}&redirect_uri=http://localhost:5000[/]");

app.Run();