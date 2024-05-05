using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class RoleDbo : IdentityRole<Guid>, IEntityTypeConfiguration<RoleDbo>
{
    public List<UserDbo> Users { get; set; } = [];
    public List<RoleClaimDbo> Claims { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<RoleDbo> builder)
    {
        builder.ToTable("role", Schemas.Identity);
        
        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<UserRoleDbo>(
                configureLeft: ur => ur
                    .HasOne(x => x.Role)
                    .WithMany()
                    .HasForeignKey(x => x.RoleId),
                configureRight: ur => ur
                    .HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
            );
    }
}