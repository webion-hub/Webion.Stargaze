using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimePackageRateDbo : IEntityTypeConfiguration<TimePackageRateDbo>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TimePackageId { get; set; }
    
    public decimal Rate { get; set; }
    
    public UserDbo User { get; set; } = null!;
    public TimePackageDbo TimePackage { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TimePackageRateDbo> builder)
    {
        builder.ToTable("time_package_rate", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rate).IsRequired();

        builder
            .HasOne(x => x.TimePackage)
            .WithMany(x => x.Rates)
            .HasForeignKey(x => x.TimePackageId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.TimePackageRates)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}