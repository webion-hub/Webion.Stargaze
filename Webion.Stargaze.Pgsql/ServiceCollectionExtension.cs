using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Webion.Stargaze.Pgsql;

public static class ServiceCollectionExtension
{
    public static void AddStargazeDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StargazeDbContext>(options =>
        {
            options.UseNpgsql(connectionString, x =>
            {
                x.MigrationsAssembly("Webion.Stargaze.Pgsql.Migrations");
            });
        });
    }
}