using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class TaskDbo : IEntityBase, IEntityTypeConfiguration<TaskDbo>
{
    public string IdPrefix => "task";
    public TypeId Id { get; set; }
    public TypeId ProjectId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    
    public ProjectDbo Project { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<TaskDbo> builder)
    {
        builder.ToTable("task", Schemas.Projects, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(4096);

        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}