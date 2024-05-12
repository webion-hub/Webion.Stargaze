using Webion.AspNetCore;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Api.Config;

public sealed class StorageConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        var conn = builder.Configuration.GetConnectionString("db")!;
        builder.Services.AddStargazeDbContext(conn);
    }

    public void Use(WebApplication app)
    {
    }
}