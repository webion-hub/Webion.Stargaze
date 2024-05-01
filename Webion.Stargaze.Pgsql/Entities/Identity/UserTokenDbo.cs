using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserTokenDbo : IdentityUserToken<TypeId>, IEntityTypeConfiguration<UserTokenDbo>
{
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<UserTokenDbo> builder)
    {
        builder.ToTable("user_tokens", Schemas.Identity);
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}