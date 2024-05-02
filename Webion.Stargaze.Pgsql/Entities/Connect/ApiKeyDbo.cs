using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.Connect;

public sealed class ApiKeyDbo : IEntityBase, IEntityTypeConfiguration<ApiKeyDbo>
{
    public string IdPrefix => "apiKey";
    public TypeId Id { get; set; }
    public TypeId ClientId { get; set; }
    
    public byte[] Secret { get; set; } = null!;
    
    public ClientDbo Client { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<ApiKeyDbo> builder)
    {
        builder.ToTable("api_key", Schemas.Connect, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);
        
        builder.Property(x => x.Secret).HasMaxLength(512).IsRequired();

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.ApiKeys)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}