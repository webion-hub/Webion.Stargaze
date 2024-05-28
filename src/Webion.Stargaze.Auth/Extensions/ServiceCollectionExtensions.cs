using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddStargazeIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<UserDbo, RoleDbo>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<StargazeDbContext>()
            .AddDefaultTokenProviders();
    }
}