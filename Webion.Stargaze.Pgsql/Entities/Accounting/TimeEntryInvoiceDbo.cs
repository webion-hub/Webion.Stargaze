using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class TimeEntryInvoiceDbo : IEntityTypeConfiguration<TimeEntryInvoiceDbo>
{
    public Guid Id { get; set; }
    public Guid TimeInvoiceId { get; set; }
    public Guid TimeEntryId { get; set; }
    
    public TimeSpan BilledTime { get; set; }
    
    public TimeInvoiceDbo TimeInvoice { get; set; } = null!;
    public TimeEntryDbo TimeEntry { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TimeEntryInvoiceDbo> builder)
    {
        builder.ToTable("time_entry_invoice", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BilledTime).IsRequired();

        builder
            .HasOne(x => x.TimeInvoice)
            .WithMany(x => x.BilledEntries)
            .HasForeignKey(x => x.TimeInvoiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.TimeEntry)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.TimeEntryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}