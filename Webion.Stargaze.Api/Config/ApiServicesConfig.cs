using Webion.AspNetCore;
using Webion.ClickUp.Sync.Synchronization;
using Webion.Stargaze.Services.Link;

namespace Qubi.Api.Config;

public sealed class ApiServicesConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {

        builder.Services.AddTransient<ClickUpLinkerService>();
        builder.Services.AddTransient<ClickUpProjectTasksSynchronizer>();
    }

    public void Use(WebApplication app)
    {
    }
}