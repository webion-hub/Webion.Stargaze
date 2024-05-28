using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Pgsql.Entities.Connect;

public sealed class RedirectUriDbo : IEntityTypeConfiguration<RedirectUriDbo>
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }

    public string Uri { get; set; } = null!;
    public StringMatch Match { get; set; }
    public RedirectUriKind Kind { get; set; }

    public ClientDbo Client { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<RedirectUriDbo> builder)
    {
        builder.ToTable("redirect_uri", Schemas.Connect);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Uri).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Match).IsRequired();
        builder.Property(x => x.Kind).IsRequired();

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.RedirectUris)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}