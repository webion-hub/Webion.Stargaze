using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class HourlyPackageDbo : IEntityTypeConfiguration<HourlyPackageDbo>
{
    public Guid Id { get; set; }

    public int Hours { get; set; }
    public decimal Rate { get; set; }

    public void Configure(EntityTypeBuilder<HourlyPackageDbo> builder)
    {
        builder.ToTable("hourly_package", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Hours).IsRequired();
        builder.Property(x => x.Rate).IsRequired();
    }
}