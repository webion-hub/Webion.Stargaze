using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Accounting;
using Webion.Stargaze.Pgsql.Entities.Core;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimePackageDbo : IEntityTypeConfiguration<TimePackageDbo>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public int Hours { get; set; }
    public string? Name { get; set; }

    public CompanyDbo Company { get; set; } = null!;
    public List<ProjectDbo> AppliesTo { get; set; } = [];
    public List<TimeInvoiceDbo> Invoices { get; set; } = [];
    public List<TimePackageRateDbo> Rates { get; set; } = [];

    public void Configure(EntityTypeBuilder<TimePackageDbo> builder)
    {
        builder.ToTable("time_package", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Hours).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(512);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.TimePackages)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.AppliesTo)
            .WithMany(x => x.TimePackages)
            .UsingEntity("time_package_project");
    }
}