using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Connect;

public sealed class ClientDbo : IEntity, IEntityTypeConfiguration<ClientDbo>
{
    public string GetIdPrefix() => "client";
    public TypeId Id { get; set; }

    public string Name { get; set; } = null!;
    public byte[] Secret { get; init; } = null!;
    
    public List<ApiKeyDbo> ApiKeys { get; set; } = [];

    public void Configure(EntityTypeBuilder<ClientDbo> builder)
    {
        builder.ToTable("clients", Schemas.Connect);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Secret).HasMaxLength(512).IsRequired();
    }
}