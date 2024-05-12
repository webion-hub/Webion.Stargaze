using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Media;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class InvoiceDocumentDbo : IEntityTypeConfiguration<InvoiceDocumentDbo>
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public Guid InvoiceId { get; set; }
    
    public FileDbo File { get; set; } = null!;
    public InvoiceDbo Invoice { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<InvoiceDocumentDbo> builder)
    {
        builder.ToTable("invoice_document", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.File)
            .WithMany()
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Invoice)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}