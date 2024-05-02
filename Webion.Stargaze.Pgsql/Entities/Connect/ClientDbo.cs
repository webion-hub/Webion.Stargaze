using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.Connect;

public sealed class ClientDbo : IEntityBase, IEntityTypeConfiguration<ClientDbo>
{
    public string IdPrefix => "client";
    public TypeId Id { get; set; }

    public string Name { get; set; } = null!;
    public byte[] Secret { get; init; } = null!;
    
    public List<ApiKeyDbo> ApiKeys { get; set; } = [];

    public void Configure(EntityTypeBuilder<ClientDbo> builder)
    {
        builder.ToTable("client", Schemas.Connect, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);
        
        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Secret).HasMaxLength(512).IsRequired();
    }
}