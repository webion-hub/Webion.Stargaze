using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Accounting;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimeEntryDbo : IEntityTypeConfiguration<TimeEntryDbo>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? TaskId { get; set; }
    
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration { get; set; }

    public bool Locked { get; set; }
    public bool Billable { get; set; }
    public bool Billed { get; set; }
    public bool Paid { get; set; }
    
    public UserDbo User { get; set; } = null!;
    public TaskDbo? Task { get; set; }
    public List<TimeEntryInvoiceDbo> Bills { get; set; } = [];

    public void Configure(EntityTypeBuilder<TimeEntryDbo> builder)
    {
        builder.ToTable("time_entry", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.Duration).IsRequired();

        builder.Property(x => x.Locked).IsRequired();
        builder.Property(x => x.Billable).IsRequired();
        builder.Property(x => x.Billed).IsRequired();
        builder.Property(x => x.Paid).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.TimeEntries)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Task)
            .WithMany(x => x.TimeEntries)
            .HasForeignKey(x => x.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}