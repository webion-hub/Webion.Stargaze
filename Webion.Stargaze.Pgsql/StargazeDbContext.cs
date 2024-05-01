using Microsoft.EntityFrameworkCore;
using TcKs.TypeId;
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
    
    public override int SaveChanges()
    {
        GenerateTypeIds();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        GenerateTypeIds();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    /// <summary>
    /// Generates TypeIds for newly added entities implementing IEntity interface.
    /// </summary>
    private void GenerateTypeIds()
    {
        var addedEntities = ChangeTracker.Entries()
            .Where(x => x.Entity is IEntity)
            .Where(x => x.State == EntityState.Added);

        foreach (var entityEntry in addedEntities)
        {
            if (entityEntry.Entity is not IEntity entity)
                continue;
            
            if (entity.Id != default)
                continue;
                
            entity.Id = TypeId.NewId(entity.GetIdPrefix());
        }
    }
}