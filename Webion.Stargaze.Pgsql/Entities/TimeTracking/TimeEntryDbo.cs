using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimeEntryDbo : IEntityTypeConfiguration<TimeEntryDbo>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration { get; set; }

    public bool Billable { get; set; }
    public bool Billed { get; set; }
    
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TimeEntryDbo> builder)
    {
        builder.ToTable("time_entry", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.Duration).IsRequired();

        builder.Property(x => x.Billable).IsRequired();
        builder.Property(x => x.Billed).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.TimeEntries)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}