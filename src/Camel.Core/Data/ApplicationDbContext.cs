using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Stats> Stats { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<Beatmap> Beatmaps { get; set; }
    public DbSet<LoginSession> LoginSessions { get; set; }
    public DbSet<Relationship> Relationships { get; set; }

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

            entity.Property(u => u.Country)
                .HasMaxLength(2)
                .HasDefaultValue("XX");
        });

        builder.Entity<Role>(entity => { entity.ToTable("roles"); });

        builder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(key => new { key.UserId, key.RoleId });
        });

        builder.Entity<IdentityUserClaim<int>>(entity => { entity.ToTable("user_claims"); });

        builder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("user_logins");
            entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });
        });

        builder.Entity<IdentityRoleClaim<int>>(entity => { entity.ToTable("role_claims"); });

        builder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("user_tokens");
            entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
        });

        builder.Entity<Stats>(entity =>
        {
            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.Stats)
                .HasForeignKey(s => s.UserId);

            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.Mode);
            entity.HasIndex(s => s.Pp);
            entity.HasIndex(s => s.TotalScore);
            entity.HasIndex(s => s.RankedScore);
        });

        builder.Entity<Score>(entity =>
        {
            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.Scores)
                .HasForeignKey(u => u.UserId);
            
            entity.HasOne<Beatmap>(s => s.Beatmap)
                .WithMany(b => b.Scores)
                .HasForeignKey(u => u.MapMd5);

            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.MapMd5);
            entity.HasIndex(s => s.ScoreNum);
            entity.HasIndex(s => s.Pp);
            entity.HasIndex(s => s.Mods);
            entity.HasIndex(s => s.Status);
            entity.HasIndex(s => s.Mode);
            entity.HasIndex(s => s.OnlineChecksum).IsUnique();
        });

        builder.Entity<Beatmap>(entity =>
        {
            entity.HasKey(b => b.Md5);
            
            entity.HasIndex(e => e.Id);
            entity.HasIndex(e => e.MapsetId);
            entity.HasIndex(e => e.Md5).IsUnique();
            entity.HasIndex(e => e.FileName);
            entity.HasIndex(e => e.Mode);
        });

        builder.Entity<LoginSession>(entity =>
        {
            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.LoginSessions)
                .HasForeignKey(s => s.UserId);

            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.DateTime);
            entity.HasIndex(s => s.OsuVersion);
        });

        builder.Entity<Relationship>(entity =>
        {
            entity.HasKey(r => new { r.FirstUserId, r.SecondUserId });

            entity.HasOne<User>(r => r.FirstUser)
                .WithMany(u => u.Added)
                .HasForeignKey(r => r.FirstUserId);

            entity.HasOne<User>(r => r.SecondUser)
                .WithMany(u => u.AddedBy)
                .HasForeignKey(r => r.SecondUserId);
        });
    }
}