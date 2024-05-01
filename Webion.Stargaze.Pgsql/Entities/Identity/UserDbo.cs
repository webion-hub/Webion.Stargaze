using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TcKs.TypeId;
using Webion.Stargaze.Pgsql.Entities.Connect;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserDbo : IdentityUser<TypeId>, IEntity, IEntityTypeConfiguration<UserDbo>
{
    public string GetIdPrefix() => "user";
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required bool Enabled { get; set; }


    public List<RoleDbo> Roles { get; set; } = [];
    public List<UserClaimDbo> Claims { get; set; } = [];
    public List<UserLoginDbo> Logins { get; set; } = [];
    public List<UserTokenDbo> Tokens { get; set; } = [];
    public List<RefreshTokenDbo> RefreshTokens { get; set; } = [];
    
    public void Configure(EntityTypeBuilder<UserDbo> builder)
    {
        builder.ToTable("users", Schemas.Identity);
    }
}