using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Accounting;

public sealed class BankAccountDbo : IEntityTypeConfiguration<BankAccountDbo>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<PaymentDbo> Payments { get; set; } = [];

    public void Configure(EntityTypeBuilder<BankAccountDbo> builder)
    {
        builder.ToTable("bank_account", Schemas.Accounting);
        builder.HasKey(x => x.Id);
    }
}