using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql.Converters;
using Webion.Stargaze.Pgsql.Entities;

namespace Webion.Stargaze.Pgsql;

public sealed class StargazeDbContext : DbContext
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
        builder.Properties<TypeId>().HaveConversion<TypeIdConverter>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.UseSnakeCaseNamingConvention();
    }
}