using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Pgsql.Entities.Connect;


public sealed class RefreshTokenDbo : IEntityTypeConfiguration<RefreshTokenDbo>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public required byte[] Secret { get; set; }
    public required DateTime ExpiresAt { get; set; }
    
    public UserDbo User { get; set; } = null!;

    
    public void Configure(EntityTypeBuilder<RefreshTokenDbo> builder)
    {
        builder.ToTable("refresh_token", Schemas.Connect);
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Secret).HasMaxLength(256).IsRequired();
        builder.Property(r => r.ExpiresAt).IsRequired();

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}