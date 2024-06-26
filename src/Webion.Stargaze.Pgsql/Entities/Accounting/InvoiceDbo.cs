using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Core.Enums;
using Webion.Stargaze.Pgsql.Entities.Core;
using Webion.Stargaze.Pgsql.Entities.Media;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class InvoiceDbo : IEntityTypeConfiguration<InvoiceDbo>
{
    public Guid Id { get; set; }
    public Guid? IssuedById { get; set; }
    public Guid? IssuedToId { get; set; }
    
    public decimal NetPrice { get; set; }
    public decimal TaxedPrice { get; set; }
    public decimal VatPercentage { get; set; }
    
    public bool Paid { get; set; }
    
    public MovementType Type { get; set; }
    public DateTimeOffset EmittedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    
    public CompanyDbo? IssuedBy { get; set; }
    public CompanyDbo? IssuedTo { get; set; }
    public List<FileDbo> Documents { get; set; } = [];
    public List<PaymentDbo> Payments { get; set; } = [];
    public List<TimeInvoiceDbo> TimeInvoices { get; set; } = [];
    public List<InvoiceItemDbo> Items { get; set; } = [];

    public void Configure(EntityTypeBuilder<InvoiceDbo> builder)
    {
        builder.ToTable("invoice", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NetPrice).IsRequired();
        builder.Property(x => x.TaxedPrice).IsRequired();
        builder.Property(x => x.VatPercentage).IsRequired();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.EmittedAt).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        
        builder
            .HasOne(x => x.IssuedBy)
            .WithMany(x => x.IssuedInvoices)
            .HasForeignKey(x => x.IssuedById)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(x => x.IssuedTo)
            .WithMany(x => x.ReceivedInvoices)
            .HasForeignKey(x => x.IssuedToId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.Documents)
            .WithMany()
            .UsingEntity("invoice_document");
    }
}