using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Pgsql;

public sealed class StargazeDbContext : IdentityDbContext<UserDbo, RoleDbo, Guid, UserClaimDbo, UserRoleDbo, UserLoginDbo, RoleClaimDbo, UserTokenDbo>
{
    public StargazeDbContext(DbContextOptions<StargazeDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(StargazeDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        builder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.UseSnakeCaseNamingConvention();
    }
}