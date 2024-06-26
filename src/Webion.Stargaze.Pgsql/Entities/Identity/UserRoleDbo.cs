using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserRoleDbo : IdentityUserRole<Guid>, IEntityTypeConfiguration<UserRoleDbo>
{
    public UserDbo User { get; set; } = null!;
    public RoleDbo Role { get; set; } = null!;


    public void Configure(EntityTypeBuilder<UserRoleDbo> builder)
    {
        builder.ToTable("user_role", Schemas.Identity);
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}