using FastIDs.TypeId;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class RoleClaimDbo : IdentityRoleClaim<TypeId>, IEntityTypeConfiguration<RoleClaimDbo>
{
    public RoleDbo Role { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<RoleClaimDbo> builder)
    {
        builder.ToTable("role_claim", Schemas.Identity);

        builder
            .HasOne(rc => rc.Role)
            .WithMany(r => r.Claims)
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}