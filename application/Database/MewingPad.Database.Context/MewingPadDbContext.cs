using Microsoft.EntityFrameworkCore;
using MewingPad.Database.Models;

namespace MewingPad.Database.Context;

public class MewingPadDbContext(DbContextOptions<MewingPadDbContext> options) : DbContext(options)
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<PlaylistDbModel> Playlists { get; set; }
    public DbSet<AudiofileDbModel> Audiofiles { get; set; }
    public DbSet<CommentaryDbModel> Commentaries { get; set; }
    public DbSet<ScoreDbModel> Scores { get; set; }
    public DbSet<ReportDbModel> Reports { get; set; }
    public DbSet<TagDbModel> Tags { get; set; }

    public DbSet<PlaylistAudiofileDbModel> PlaylistsAudiofiles { get; set; }
    public DbSet<TagAudiofileDbModel> TagsAudiofiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScoreDbModel>().HasKey(u => new { u.AuthorId, u.AudiofileId });

        modelBuilder.Entity<AudiofileDbModel>()
            .HasMany(e => e.Playlists)
            .WithMany(e => e.Audiofiles)
            .UsingEntity<PlaylistAudiofileDbModel>(
                l => l.HasOne(e => e.Playlist).WithMany(e => e.PlaylistsAudiofiles),
                r => r.HasOne(e => e.Audiofile).WithMany(e => e.PlaylistsAudiofiles)
            );

        modelBuilder.Entity<AudiofileDbModel>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Audiofiles)
            .UsingEntity<TagAudiofileDbModel>(
                l => l.HasOne(e => e.Tag).WithMany(e => e.TagsAudiofiles),
                r => r.HasOne(e => e.Audiofile).WithMany(e => e.TagsAudiofiles)
            );

        base.OnModelCreating(modelBuilder);
    }
}
