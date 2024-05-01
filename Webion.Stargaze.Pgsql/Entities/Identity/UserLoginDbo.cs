using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserLoginDbo : IdentityUserLogin<TypeId>, IEntityTypeConfiguration<UserLoginDbo>
{
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<UserLoginDbo> builder)
    {
        builder.ToTable("user_login", Schemas.Identity);
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
        
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.Logins)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}