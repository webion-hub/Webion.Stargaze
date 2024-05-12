using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql;

public sealed class StargazeDbContext : IdentityDbContext<UserDbo, RoleDbo, Guid, UserClaimDbo, UserRoleDbo, UserLoginDbo, RoleClaimDbo, UserTokenDbo>
{
    public StargazeDbContext(DbContextOptions<StargazeDbContext> options) : base(options)
    {
    }
    
    
    public DbSet<TimeEntryDbo> TimeEntries { get; set; }
    
    public DbSet<ClientDbo> Clients { get; set; } = null!;
    public DbSet<ApiKeyDbo> ApiKeys { get; set; } = null!;
    public DbSet<RefreshTokenDbo> RefreshTokens { get; set; } = null!;
    

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