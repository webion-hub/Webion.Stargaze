using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Projects;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Pgsql.Entities.Identity;

public sealed class UserDbo : IdentityUser<Guid>, IEntityTypeConfiguration<UserDbo>
{
    public List<RoleDbo> Roles { get; set; } = [];
    public List<UserClaimDbo> Claims { get; set; } = [];
    public List<UserLoginDbo> Logins { get; set; } = [];
    public List<UserTokenDbo> Tokens { get; set; } = [];
    public List<RefreshTokenDbo> RefreshTokens { get; set; } = [];
    
    public List<TaskDbo> Tasks { get; set; }
    public List<TimeEntryDbo> TimeEntries { get; set; } = [];
    public List<TimePackageRateDbo> TimePackageRates { get; set; } = [];

    public void Configure(EntityTypeBuilder<UserDbo> builder)
    {
        builder.ToTable("user", Schemas.Identity);
    }
}