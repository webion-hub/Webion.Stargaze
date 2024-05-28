using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserTokenDbo : IdentityUserToken<Guid>, IEntityTypeConfiguration<UserTokenDbo>
{
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<UserTokenDbo> builder)
    {
        builder.ToTable("user_token", Schemas.Identity);
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        builder.Property(x => x.IssuedAt).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}