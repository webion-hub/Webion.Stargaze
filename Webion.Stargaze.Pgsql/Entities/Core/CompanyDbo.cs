using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Accounting;
using Webion.Stargaze.Pgsql.Entities.Projects;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Core;

public sealed class CompanyDbo : IEntityTypeConfiguration<CompanyDbo>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<InvoiceDbo> IssuedInvoices { get; set; } = [];
    public List<InvoiceDbo> ReceivedInvoices { get; set; } = [];
    public List<ProjectDbo> Projects { get; set; } = [];
    public List<TimePackageDbo> TimePackages { get; set; } = [];

    public void Configure(EntityTypeBuilder<CompanyDbo> builder)
    {
        builder.ToTable("company", Schemas.Core);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
    }
}