using FastIDs.TypeId;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserClaimDbo : IdentityUserClaim<TypeId>, IEntityTypeConfiguration<UserClaimDbo>
{
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<UserClaimDbo> builder)
    {
        builder.ToTable("user_claim", Schemas.Identity);
        builder.HasKey(x => x.Id);
        
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.Claims)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}