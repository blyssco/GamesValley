using GamesValley.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GamesValley.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("dbo");

            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity => {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity => {
                entity.ToTable(name: "UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity => {
                entity.ToTable(name: "UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity => {
                entity.ToTable(name: "UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity => {
                entity.ToTable(name: "RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity => {
                entity.ToTable(name: "UserTokens");
            });

            builder.Entity<ApplicationUser>()
            .HasMany(u => u.Games)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId);
        }
        public DbSet<Games> Games { get; set; }
    }
}