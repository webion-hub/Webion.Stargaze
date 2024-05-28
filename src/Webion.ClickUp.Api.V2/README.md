# Overview
This package adds support for the ClickUp v2 api.

# Usage
The recommended approach is to register the api inside a service collection, and then inject it inside your services.

## Example
```csharp
builder.Services
    .AddClickUpApi()
    .ConfigureHttpClient(x =>
    {
        x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            builder.Configuration["ClickUp:ApiKey"]!
        );
    });
```

```csharp
public sealed class MyService
{
    private readonly IClickUpApi _clickUpApi;
    
    public MyService(IClickUpApi clickUpApi)
    {
        _clickUpApi = clickUpApi;
    }
    
    public async Task DoSomething()
    {
        var getAllTeamsResponse = await _clickUpApi.Teams.GetAllAsync();
    }
}
```

# Authentication
You can either add an api key like in the example above, or leverage the http delegating handlers to send an AccessToken.

## Access token example
```csharp
public sealed class ClickUpApiAuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClickUpApiAuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Example of retrieving the access token from the user's claims
        var user = _httpContextAccessor.HttpContext?.User;
        var token = user?.FindFirstValue("clickup_access_token");
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(token);
        }
        
        return base.SendAsync(request, cancellationToken);
    }
}

// Registering the handler
builder.Services.AddTransient<ClickUpApiAuthHandler>();
builder.Services
    .AddClickUpApi()
    .AddHttpMessageHandler<ClickUpApiAuthHandler>();
```