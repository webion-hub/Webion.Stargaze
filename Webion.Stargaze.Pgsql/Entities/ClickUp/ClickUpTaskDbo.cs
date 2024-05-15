using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public sealed class ClickUpTaskDbo : IEntityTypeConfiguration<ClickUpTaskDbo>
{
    public string Id { get; set; } = null!;
    public string ListId { get; set; } = null!;
    public Guid? TaskId { get; set; }

    public TaskDbo? Task { get; set; }
    public ClickUpListDbo List { get; set; } = null!;
    public List<ClickUpTimeEntryDbo> ClickUpTimeEntries { get; set; } = [];

    public void Configure(EntityTypeBuilder<ClickUpTaskDbo> builder)
    {
        builder.ToTable("task", Schemas.ClickUp);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne(x => x.Task)
            .WithOne(x => x.ClickUpTask)
            .HasForeignKey<ClickUpTaskDbo>(x => x.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder
            .HasOne(x => x.List)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}