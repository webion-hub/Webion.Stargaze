using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class TaskDbo : IEntityTypeConfiguration<TaskDbo>
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    
    public ProjectDbo Project { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TaskDbo> builder)
    {
        builder.ToTable("task", Schemas.Projects);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(4096);

        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}