using System.Net.Http.Headers;
using System.Security.Claims;

namespace Webion.Stargaze.Api;

public sealed class ClickUpApiAuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClickUpApiAuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var token = user?.FindFirstValue("clickup_access_token");
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(token);
        }
        
        return base.SendAsync(request, cancellationToken);
    }
}