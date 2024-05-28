using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class PaymentCategoryDbo : IEntityTypeConfiguration<PaymentCategoryDbo>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public List<PaymentDbo> Payments { get; set; } = [];

    public void Configure(EntityTypeBuilder<PaymentCategoryDbo> builder)
    {
        builder.ToTable("payment_category", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(512).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4096);
    }
}