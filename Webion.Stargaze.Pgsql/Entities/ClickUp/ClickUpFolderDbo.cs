using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public sealed class ClickUpFolderDbo : IEntityTypeConfiguration<ClickUpFolderDbo>
{
    public string Id { get; set; } = null!;
    public string SpaceId { get; set; } = null!;

    public string? Name { get; set; }
    
    public ClickUpSpaceDbo Space { get; set; } = null!;
    public List<ClickUpListDbo> Lists { get; set; } = [];

    public void Configure(EntityTypeBuilder<ClickUpFolderDbo> builder)
    {
        builder.ToTable("folder", Schemas.ClickUp);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne(x => x.Space)
            .WithMany(x => x.Folders)
            .HasForeignKey(x => x.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}