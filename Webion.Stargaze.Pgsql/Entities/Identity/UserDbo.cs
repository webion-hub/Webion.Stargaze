using FastIDs.TypeId;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;
using Webion.Stargaze.Pgsql.Extensions;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserDbo : IdentityUser<TypeId>, IEntityBase, IEntityTypeConfiguration<UserDbo>
{
    public string IdPrefix => "user";
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool Enabled { get; set; }


    public List<RoleDbo> Roles { get; set; } = [];
    public List<UserClaimDbo> Claims { get; set; } = [];
    public List<UserLoginDbo> Logins { get; set; } = [];
    public List<UserTokenDbo> Tokens { get; set; } = [];
    public List<RefreshTokenDbo> RefreshTokens { get; set; } = [];
    public List<TimeEntryDbo> TimeEntries { get; set; } = [];

    public void Configure(EntityTypeBuilder<UserDbo> builder)
    {
        builder.ToTable("user", Schemas.Identity, b =>
        {
            b.HasTypeIdCheckConstraint(IdPrefix);
        });
        
        builder.Property(x => x.Id).HasDefaultTypeIdValue(IdPrefix);
    }
}