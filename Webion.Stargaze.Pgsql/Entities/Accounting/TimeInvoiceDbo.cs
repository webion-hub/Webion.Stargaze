using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class TimeInvoiceDbo : IEntityTypeConfiguration<TimeInvoiceDbo>
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid? HourlyPackageId { get; set; }
    
    public TimeSpan InvoicedAmount { get; set; }

    public InvoiceDbo Invoice { get; set; } = null!;
    public HourlyPackageDbo? HourlyPackage { get; set; }
    public List<TimeEntryInvoiceDbo> BilledEntries { get; set; } = [];

    public void Configure(EntityTypeBuilder<TimeInvoiceDbo> builder)
    {
        builder.ToTable("time_invoice", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Invoice)
            .WithMany(x => x.TimeInvoices)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.HourlyPackage)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.HourlyPackageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}