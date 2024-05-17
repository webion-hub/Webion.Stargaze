using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class PaymentDbo : IEntityTypeConfiguration<PaymentDbo>
{
    public Guid Id { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? CategoryId { get; set; }
    
    
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Description { get; set; }
    
    public decimal Amount { get; set; }
    public MovementType Type { get; set; }
    public PaymentStatus Status { get; set; }
    
    
    public DateTimeOffset PaidAt { get; set; }
    
    public InvoiceDbo? Invoice { get; set; }
    public BankAccountDbo? BankAccount { get; set; }
    public PaymentCategoryDbo? Category { get; set; }
    
    public void Configure(EntityTypeBuilder<PaymentDbo> builder)
    {
        builder.ToTable("payment", Schemas.Accounting);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description).HasMaxLength(4096);
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder
            .HasOne(x => x.Invoice)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.InvoiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.BankAccount)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.BankAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}