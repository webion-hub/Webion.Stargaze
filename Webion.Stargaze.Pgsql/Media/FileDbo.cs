using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Media;

public sealed class FileDbo : IEntityTypeConfiguration<FileDbo>
{
    public Guid Id { get; set; }
    
    public void Configure(EntityTypeBuilder<FileDbo> builder)
    {
        builder.ToTable("file", Schemas.Media);
        builder.HasKey(x => x.Id);
    }
}