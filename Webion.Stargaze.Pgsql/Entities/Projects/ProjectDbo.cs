using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Core;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Projects;

public sealed class ProjectDbo : IEntityTypeConfiguration<ProjectDbo>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }


    public CompanyDbo Company { get; set; } = null!;
    public List<TaskDbo> Tasks { get; set; } = [];
    public List<TimePackageDbo> TimePackages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ProjectDbo> builder)
    {
        builder.ToTable("project", Schemas.Projects);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(4096);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}