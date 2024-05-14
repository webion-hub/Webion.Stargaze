using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql.Entities.ClickUp;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Core;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.Projects;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql;

public sealed class StargazeDbContext : IdentityDbContext<UserDbo, RoleDbo, Guid, UserClaimDbo, UserRoleDbo, UserLoginDbo, RoleClaimDbo, UserTokenDbo>
{
    public StargazeDbContext(DbContextOptions<StargazeDbContext> options) : base(options)
    {
    }


    public DbSet<CompanyDbo> Companies { get; set; }
    public DbSet<ProjectDbo> Projects { get; set; }
    public DbSet<TaskDbo> Tasks { get; set; }
    public DbSet<TimeEntryDbo> TimeEntries { get; set; }
    public DbSet<TimePackageDbo> TimePackages { get; set; }

    public DbSet<ClientDbo> Clients { get; set; }
    public DbSet<ApiKeyDbo> ApiKeys { get; set; }
    public DbSet<RefreshTokenDbo> RefreshTokens { get; set; }
    public DbSet<TimePackageRateDbo> TimePackageRates { get; set; }
    
    public DbSet<ClickUpSpaceDbo> ClickUpSpaces { get; set; }
    public DbSet<ClickUpFolderDbo> ClickUpFolders { get; set; }
    public DbSet<ClickUpListDbo> ClickUpLists { get; set; }
    public DbSet<ClickUpTaskDbo> ClickUpTasks { get; set; }
    public DbSet<ClickUpTimeEntryDbo> ClickUpTimeEntries { get; set; }


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