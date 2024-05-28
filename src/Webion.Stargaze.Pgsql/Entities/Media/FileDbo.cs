using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Pgsql.Entities.Media;

public sealed class FileDbo : IEntityTypeConfiguration<FileDbo>
{
    public Guid Id { get; set; }

    public string Path { get; set; } = null!;
    public StorageProvider Provider { get; set; }
    
    public void Configure(EntityTypeBuilder<FileDbo> builder)
    {
        builder.ToTable("file", Schemas.Media);
        builder.HasKey(x => x.Id);
    }
}