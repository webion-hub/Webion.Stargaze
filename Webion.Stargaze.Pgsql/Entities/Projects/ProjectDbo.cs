using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class ProjectDbo : IEntity, IEntityTypeConfiguration<ProjectDbo>
{
    public string GetIdPrefix() => "project";
    public TypeId Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }


    public List<TaskDbo> Tasks { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<ProjectDbo> builder)
    {
        builder.ToTable("project", Schemas.Projects);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(4096);
    }
}