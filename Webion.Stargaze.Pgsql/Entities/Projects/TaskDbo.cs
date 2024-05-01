using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class TaskDbo : IEntity, IEntityTypeConfiguration<TaskDbo>
{
    public string GetIdPrefix() => "task";
    public TypeId Id { get; set; }
    public TypeId ProjectId { get; set; }

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