using FastIDs.TypeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class ProjectDbo : IEntityBase, IEntityTypeConfiguration<ProjectDbo>
{
    public string IdPrefix => "project";
    public TypeId Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }


    public List<TaskDbo> Tasks { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<ProjectDbo> builder)
    {
        builder.ToTable("project", Schemas.Projects, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(4096);
    }
}