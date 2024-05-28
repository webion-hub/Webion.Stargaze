# Overview
Adds ClickUp as an authentication handler for AspNetCore.

# Usage
```csharp
// Registering the handler
builder.Services
    .AddAuthentication()
    .AddClickUp(options =>
    {
        options.ClientId = builder.Configuration["ClickUp:ClientId"]!;
        options.ClientSecret = builder.Configuration["ClickUp:ClientSecret"]!;
    });

// Usage
public sealed class LoginController : ControllerBase
{
    public IActionResult Login()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("Callback"),
        };
        
        // Redirects the user to clickup's sign in page.
        return Challenge(properties, ClickUpDefaults.AuthenticationScheme);
    }
    
    public IActionResult Callback()
    {
        // Gets the username
        var userName = User.FindFirstValue(ClaimTypes.Name);
    }
}
```

# Mapped claims

- `ClaimTypes.NameIdentifier`: ClickUp user id
- `ClaimTypes.Name`: ClickUp username
- `ClickUpClaims.ProfilePicture`
- `ClickUpClaims.Color`
- `ClickUpClaims.AccessToken`