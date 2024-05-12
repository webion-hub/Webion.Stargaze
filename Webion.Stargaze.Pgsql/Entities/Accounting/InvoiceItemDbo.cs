using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class InvoiceItemDbo : IEntityTypeConfiguration<InvoiceItemDbo>
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    
    public decimal TotalUnits { get; set; } 
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    public decimal TaxedPrice { get; set; }
    public decimal VatPercentage { get; set; }
    
    public InvoiceDbo Invoice { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<InvoiceItemDbo> builder)
    {
        builder.ToTable("invoice_item", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalUnits).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4096);
        
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.TaxedPrice).IsRequired();
        builder.Property(x => x.VatPercentage).IsRequired();

        builder
            .HasOne(x => x.Invoice)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}