using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public sealed class ClickUpSpaceDbo : IEntityTypeConfiguration<ClickUpSpaceDbo>
{
    public string Id { get; set; } = null!;
    
    public string? Name { get; set; }

    public List<ClickUpListDbo> Lists { get; set; } = [];
    public List<ClickUpFolderDbo> Folders { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<ClickUpSpaceDbo> builder)
    {
        builder.ToTable("space", Schemas.ClickUp);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}