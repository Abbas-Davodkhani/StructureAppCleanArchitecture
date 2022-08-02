using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class IdentityDatabaseContext : IdentityDbContext<User>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            string schema = "identity";

            builder.Entity<IdentityUser<string>>().ToTable("Users", schema);
            builder.Entity<IdentityRole<string>>().ToTable("Roles", schema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims" , schema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schema);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schema);


            builder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider , p.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });

            //base.OnModelCreating(builder);
        }
    }
}
