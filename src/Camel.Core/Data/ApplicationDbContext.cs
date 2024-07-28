using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Camel.Core.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Stats> Stats { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<Beatmap> Beatmaps { get; set; }
    public DbSet<LoginSession> LoginSessions { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Comment> Comments { get; set; }

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

            entity.HasOne<Profile>(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.Id);
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
            entity.Property(s => s.MapMd5).HasMaxLength(32);
            entity.Property(s => s.OnlineChecksum).HasMaxLength(32);

            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.Scores)
                .HasForeignKey(u => u.UserId);

            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.MapMd5);
            entity.HasIndex(s => s.ScoreNum);
            entity.HasIndex(s => s.Pp);
            entity.HasIndex(s => s.Mods);
            entity.HasIndex(s => s.Status);
            entity.HasIndex(s => s.Mode);
            entity.HasIndex(s => s.OnlineChecksum).IsUnique();
            entity.HasIndex(s => s.SessionId);
            entity.HasIndex(s => new { s.MapMd5, s.Status, s.Mode });
        });

        builder.Entity<Beatmap>(entity =>
        {
            entity.Property(b => b.Md5).HasMaxLength(32);
            entity.Property(b => b.Artist).HasMaxLength(128);
            entity.Property(b => b.Title).HasMaxLength(128);
            entity.Property(b => b.ArtistUnicode).HasMaxLength(128);
            entity.Property(b => b.TitleUnicode).HasMaxLength(128);
            entity.Property(b => b.Version).HasMaxLength(128);
            entity.Property(b => b.Source).HasMaxLength(128);
            entity.Property(b => b.FileName).HasMaxLength(256);
            entity.Property(b => b.Creator).HasMaxLength(32);

            entity.HasIndex(b => b.Id);
            entity.HasIndex(b => b.MapsetId);
            entity.HasIndex(b => b.Md5).IsUnique();
            entity.HasIndex(b => b.FileName);
            entity.HasIndex(b => b.Mode);
        });

        builder.Entity<LoginSession>(entity =>
        {
            entity.Property(s => s.OsuVersion).HasMaxLength(32);
            entity.Property(s => s.OsuPathMd5).HasMaxLength(32);
            entity.Property(s => s.AdaptersStr).HasMaxLength(128);
            entity.Property(s => s.AdaptersMd5).HasMaxLength(32);
            entity.Property(s => s.DiskSignatureMd5).HasMaxLength(32);
            entity.Property(s => s.IpAddress).HasMaxLength(32);

            entity.HasOne<User>(s => s.User)
                .WithMany(u => u.LoginSessions)
                .HasForeignKey(s => s.UserId);

            entity.HasIndex(s => s.UserId);
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

        builder.Entity<Profile>(entity =>
        {
            entity.Property(p => p.Twitter).HasMaxLength(64);
            entity.Property(p => p.Discord).HasMaxLength(64);
            entity.Property(p => p.UserPage).HasMaxLength(1024);
        });

        builder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Text).HasMaxLength(300);

            entity.HasOne<User>(c => c.Author)
                .WithMany(a => a.PostedComments)
                .HasForeignKey(c => c.AuthorId);
            
            entity.HasOne<User>(c => c.User)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.UserId);
            
            entity.HasOne<Beatmap>(c => c.Beatmap)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BeatmapId);
            
            entity.HasOne<Score>(c => c.Score)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.ScoreId);
        });
    }
}