using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.TimeTracking;

public sealed class TimeEntryDbo : IEntity, IEntityTypeConfiguration<TimeEntryDbo>
{
    public string GetIdPrefix() => "time_entry";
    public TypeId Id { get; set; }
    
    public TypeId UserId { get; set; }
    
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration { get; set; }
    
    
    
    public void Configure(EntityTypeBuilder<TimeEntryDbo> builder)
    {
        builder.ToTable("time_entry", Schemas.TimeTracking);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.Duration).IsRequired();
    }
}