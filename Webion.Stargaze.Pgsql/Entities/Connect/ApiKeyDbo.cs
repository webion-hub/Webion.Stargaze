using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Connect;

public sealed class ApiKeyDbo : IEntityTypeConfiguration<ApiKeyDbo>
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    
    public byte[] Secret { get; set; } = null!;
    
    public ClientDbo Client { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<ApiKeyDbo> builder)
    {
        builder.ToTable("api_key", Schemas.Connect);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Secret).HasMaxLength(512).IsRequired();

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.ApiKeys)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}