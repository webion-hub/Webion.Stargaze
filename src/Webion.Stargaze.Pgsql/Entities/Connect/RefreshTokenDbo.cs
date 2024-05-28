using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Pgsql.Entities.Connect;


public sealed class RefreshTokenDbo : IEntityTypeConfiguration<RefreshTokenDbo>
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid UserId { get; set; }
    
    public required byte[] Secret { get; set; }
    public required DateTimeOffset ExpiresAt { get; set; }
    
    public ClientDbo Client { get; set; } = null!;
    public UserDbo User { get; set; } = null!;
    
    public void Configure(EntityTypeBuilder<RefreshTokenDbo> builder)
    {
        builder.ToTable("refresh_token", Schemas.Connect);
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Secret).HasMaxLength(256).IsRequired();
        builder.Property(r => r.ExpiresAt).IsRequired();

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}