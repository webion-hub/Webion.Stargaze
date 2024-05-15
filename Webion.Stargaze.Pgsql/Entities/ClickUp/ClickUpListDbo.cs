using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public sealed class ClickUpListDbo : IEntityTypeConfiguration<ClickUpListDbo>
{
    public string Id { get; set; } = null!;
    public string SpaceId { get; set; } = null!;
    public string? FolderId { get; set; }
    
    public string? Name { get; set; }

    public ClickUpSpaceDbo Space { get; set; } = null!;
    public ClickUpFolderDbo? Folder { get; set; }
    public List<ProjectDbo> Projects { get; set; } = [];
    public List<ClickUpTaskDbo> Tasks { get; set; } = [];

    public void Configure(EntityTypeBuilder<ClickUpListDbo> builder)
    {
        builder.ToTable("list", Schemas.ClickUp);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne(x => x.Space)
            .WithMany(x => x.Lists)
            .HasForeignKey(x => x.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.Folder)
            .WithMany(x => x.Lists)
            .HasForeignKey(x => x.FolderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder
            .HasMany(x => x.Projects)
            .WithMany(x => x.ClickUpLists)
            .UsingEntity("click_up_project_list");
    }
}