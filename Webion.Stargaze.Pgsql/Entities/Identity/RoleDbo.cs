using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class RoleDbo : IdentityRole<TypeId>, IEntity, IEntityTypeConfiguration<RoleDbo>
{
    public string GetIdPrefix() => "role";
    
    public List<UserDbo> Users { get; set; } = [];
    public List<RoleClaimDbo> Claims { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<RoleDbo> builder)
    {
        builder.ToTable("roles", Schemas.Identity);
        
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