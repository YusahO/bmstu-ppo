using Microsoft.EntityFrameworkCore;
using MewingPad.Database.PgSQL.Models;

namespace MewingPad.Database.PgSQL.Context;

public class MewingPadPgSQLDbContext(DbContextOptions<MewingPadPgSQLDbContext> options) : DbContext(options)
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<PlaylistDbModel> Playlists { get; set; }
    public DbSet<AudiotrackDbModel> Audiotracks { get; set; }
    public DbSet<CommentaryDbModel> Commentaries { get; set; }
    public DbSet<ScoreDbModel> Scores { get; set; }
    public DbSet<ReportDbModel> Reports { get; set; }
    public DbSet<TagDbModel> Tags { get; set; }

    public DbSet<PlaylistAudiotrackDbModel> PlaylistsAudiotracks { get; set; }
    public DbSet<TagAudiotrackDbModel> TagsAudiotracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudiotrackDbModel>(
            eb => eb.Property(b => b.Title).HasColumnType("varchar(64)"));
        modelBuilder.Entity<PlaylistDbModel>(
            eb => eb.Property(b => b.Title).HasColumnType("varchar(64)"));
        modelBuilder.Entity<ReportDbModel>(
            eb => eb.Property(b => b.Status).HasColumnType("varchar(50)"));
        modelBuilder.Entity<TagDbModel>(
            eb => eb.Property(b => b.Name).HasColumnType("varchar(64)"));
        modelBuilder.Entity<UserDbModel>(
            eb =>
            {
                eb.Property(b => b.Username).HasColumnType("varchar(64)");
                eb.Property(b => b.PasswordHashed).HasColumnType("varchar(128)");
                eb.Property(b => b.Email).HasColumnType("varchar(320)");
            });

        modelBuilder.Entity<ScoreDbModel>().HasKey(u => new { u.AuthorId, u.AudiotrackId });
        modelBuilder.Entity<AudiotrackDbModel>()
            .HasMany(e => e.Playlists)
            .WithMany(e => e.Audiotracks)
            .UsingEntity<PlaylistAudiotrackDbModel>(
                l => l.HasOne(e => e.Playlist).WithMany(e => e.PlaylistsAudiotracks),
                r => r.HasOne(e => e.Audiotrack).WithMany(e => e.PlaylistsAudiotracks)
            );

        modelBuilder.Entity<AudiotrackDbModel>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Audiotracks)
            .UsingEntity<TagAudiotrackDbModel>(
                l => l.HasOne(e => e.Tag).WithMany(e => e.TagsAudiotracks),
                r => r.HasOne(e => e.Audiotrack).WithMany(e => e.TagsAudiotracks)
            );

        modelBuilder.Entity<UserDbModel>()
            .HasMany(u => u.Playlists)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<UserDbModel>()
            .HasMany(u => u.Scores)
            .WithOne(s => s.Author)
            .HasForeignKey(s => s.AuthorId);

        base.OnModelCreating(modelBuilder);
    }
}
