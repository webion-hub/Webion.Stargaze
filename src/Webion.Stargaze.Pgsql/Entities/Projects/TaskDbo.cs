using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.ClickUp;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class TaskDbo : IEntityTypeConfiguration<TaskDbo>
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    
    public ProjectDbo Project { get; set; } = null!;
    public List<TimeEntryDbo> TimeEntries { get; set; } = [];
    public List<UserDbo> Assignees { get; set; } = [];
    public ClickUpTaskDbo? ClickUpTask { get; set; }

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

        builder
            .HasMany(x => x.Assignees)
            .WithMany(x => x.Tasks)
            .UsingEntity("user_task");
    }
}