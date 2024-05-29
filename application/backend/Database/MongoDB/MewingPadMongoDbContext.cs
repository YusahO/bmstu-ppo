using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.EntityFrameworkCore.Extensions;
using MewingPad.Database.Models;

namespace MewingPad.Database.MongoDB.Context;

public class MewingPadMongoDbContext(DbContextOptions<MewingPadMongoDbContext> options) : DbContext(options)
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<PlaylistDbModel> Playlists { get; set; }
    public DbSet<AudiotrackDbModel> Audiotracks { get; set; }
    public DbSet<CommentaryDbModel> Commentaries { get; set; }
    public DbSet<ScoreDbModel> Scores { get; set; }
    public DbSet<ReportDbModel> Reports { get; set; }
    public DbSet<TagDbModel> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScoreDbModel>().HasKey(u => new { u.AuthorId, u.AudiotrackId });

        modelBuilder.Entity<UserDbModel>().ToCollection("Users");
        modelBuilder.Entity<PlaylistDbModel>().ToCollection("Playlists");
        modelBuilder.Entity<AudiotrackDbModel>().ToCollection("Audiotracks");
        modelBuilder.Entity<CommentaryDbModel>().ToCollection("Commentaries");
        modelBuilder.Entity<ScoreDbModel>().ToCollection("Scores");
        modelBuilder.Entity<ReportDbModel>().ToCollection("Repors");
        modelBuilder.Entity<TagDbModel>().ToCollection("Tags");
    }
}