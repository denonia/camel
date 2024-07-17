using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Stats> Stats { get; set; }
    public DbSet<Score> Scores { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("users");
        });
        builder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
        });
        builder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(key => new { key.UserId, key.RoleId });
        });

        builder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("user_claims");
        });

        builder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("user_logins");
            entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
        });

        builder.Entity<IdentityRoleClaim<int>>(entity =>
        {
            entity.ToTable("role_claims");

        });

        builder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("user_tokens");
            entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

        });

        builder.Entity<Stats>(entity =>
        {
            entity.HasOne<User>(u => u.User)
                .WithMany(u => u.Stats)
                .HasForeignKey(u => u.UserId);
        });
        
        builder.Entity<Score>(entity =>
        {
            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.Scores)
                .HasForeignKey(u => u.UserId);
        });

    }
}