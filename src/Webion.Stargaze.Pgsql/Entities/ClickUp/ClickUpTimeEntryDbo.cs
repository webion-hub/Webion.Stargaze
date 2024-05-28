using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public sealed class ClickUpTimeEntryDbo : IEntityTypeConfiguration<ClickUpTimeEntryDbo>
{
    public string Id { get; set; } = null!;
    public string? TaskId { get; set; } 
    
    
    public ClickUpTaskDbo? ClickUpTask { get; set; }
    
    public void Configure(EntityTypeBuilder<ClickUpTimeEntryDbo> builder)
    {
        builder.ToTable("time_entry", Schemas.ClickUp);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne(x => x.ClickUpTask)
            .WithMany(x => x.ClickUpTimeEntries)
            .HasForeignKey(x => x.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}