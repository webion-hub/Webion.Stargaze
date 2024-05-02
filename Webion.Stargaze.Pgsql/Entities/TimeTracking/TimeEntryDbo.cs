using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimeEntryDbo : IEntityBase, IEntityTypeConfiguration<TimeEntryDbo>
{
    public string IdPrefix => "timeEntry";
    public TypeId Id { get; set; }
    public TypeId UserId { get; set; }
    
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration { get; set; }

    
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TimeEntryDbo> builder)
    {
        builder.ToTable("time_entry", Schemas.TimeTracking, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);
        
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.Duration).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.TimeEntries)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}