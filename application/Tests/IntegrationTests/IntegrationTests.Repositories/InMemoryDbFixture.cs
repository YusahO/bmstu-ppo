using MewingPad.Common.Entities;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace IntegrationTests.DbFixtures;

public class InMemoryDbFixture() : IDisposable
{
    public readonly MewingPadDbContext Context = new InMemoryDbContextFactory().GetDbContext();

    public async Task<User?> GetUserById(Guid userId)
    {
        return UserConverter.DbToCoreModel(await Context.Users.FindAsync(userId));
    }

    public async Task<Audiofile?> GetAudiofileById(Guid audiofileId)
    {
        return AudiofileConverter.DbToCoreModel(await Context.Audiofiles.FindAsync(audiofileId));
    }

    public async Task<Commentary?> GetCommentaryById(Guid commentaryId)
    {
        return CommentaryConverter.DbToCoreModel(await Context.Commentaries.FindAsync(commentaryId));
    }

    public async Task<Tag?> GetTagById(Guid tagId)
    {
        return TagConverter.DbToCoreModel(await Context.Tags.FindAsync(tagId));
    }

    public async Task<Report?> GetReportById(Guid reportId)
    {
        return ReportConverter.DbToCoreModel(await Context.Reports.FindAsync(reportId));
    }

    public async Task<Score?> GetScoreById(Guid scoreId)
    {
        return ScoreConverter.DbToCoreModel(await Context.Scores.FindAsync(scoreId));
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        return PlaylistConverter.DbToCoreModel(await Context.Playlists.FindAsync(playlistId));
    }

    public async Task InsertUsers(List<User> users)
    {
        foreach (var user in users)
        {
            await Context.Users.AddAsync(UserConverter.CoreToDbModel(user)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertPlaylists(List<Playlist> playlists)
    {
        foreach (var playlist in playlists)
        {
            await Context.Playlists.AddAsync(PlaylistConverter.CoreToDbModel(playlist)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertAudiofiles(List<Audiofile> audiofiles)
    {
        foreach (var audiofile in audiofiles)
        {
            await Context.Audiofiles.AddAsync(AudiofileConverter.CoreToDbModel(audiofile)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertTags(List<Tag> tags)
    {
        foreach (var tag in tags)
        {
            await Context.Tags.AddAsync(TagConverter.CoreToDbModel(tag)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertCommentaries(List<Commentary> commentaries)
    {
        foreach (var commentary in commentaries)
        {
            await Context.Commentaries.AddAsync(CommentaryConverter.CoreToDbModel(commentary)!);
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertScores(List<Score> scores)
    {
        foreach (var score in scores)
        {
            await Context.Scores.AddAsync(ScoreConverter.CoreToDbModel(score)!);
        }
    }

    public async Task InsertReports(List<Report> reports)
    {
        foreach (var report in reports)
        {
            await Context.Reports.AddAsync(ReportConverter.CoreToDbModel(report)!);
        }
    }

    public async Task InsertPlaylistsAudiofiles(List<KeyValuePair<Guid, Guid>> pairs)
    {
        foreach (var p in pairs)
        {
            await Context.PlaylistsAudiofiles.AddAsync(new(p.Key, p.Value));
        }
        await Context.SaveChangesAsync();
    }

    public async Task InsertTagsAudiofiles(List<KeyValuePair<Guid, Guid>> pairs)
    {
        foreach (var p in pairs)
        {
            await Context.TagsAudiofiles.AddAsync(new(p.Key, p.Value));
        }
        await Context.SaveChangesAsync();
    }

    public static List<User> CreateMockUsers()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), "user1", "u1@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", true, false),
            new(Guid.NewGuid(), Guid.NewGuid(), "user2", "u2@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", false, false),
            new(Guid.NewGuid(), Guid.NewGuid(), "user3", "u3@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", false, true),
            new(Guid.NewGuid(), Guid.NewGuid(), "user4", "u4@mail.ru", "$2a$11$2RL0J3feilWw6859UcNwY.dighT4cPxG/she0Omtu36eVtamkV.8y", true, true)
        ];
    }

    public static List<Playlist> CreateMockPlaylists()
    {
        return
        [
            new(Guid.NewGuid(), "title1", Guid.NewGuid()),
            new(Guid.NewGuid(), "title2", Guid.NewGuid()),
            new(Guid.NewGuid(), "title3", Guid.NewGuid()),
            new(Guid.NewGuid(), "title4", Guid.NewGuid())
        ];
    }

    public static List<Audiofile> CreateMockAudiofiles()
    {
        return
        [
            new (Guid.NewGuid(), "title1", 0.0f, Guid.NewGuid(), "path/to/file1"),
            new (Guid.NewGuid(), "title2", 0.0f, Guid.NewGuid(), "path/to/file2"),
            new (Guid.NewGuid(), "title3", 0.0f, Guid.NewGuid(), "path/to/file3")
        ];
    }

    public static List<Tag> CreateMockTags()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), "tag1"),
            new(Guid.NewGuid(), Guid.NewGuid(), "tag2"),
            new(Guid.NewGuid(), Guid.NewGuid(), "tag3")
        ];
    }

    public static List<Commentary> CreateMockCommentaries()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "3")
        ];
    }

    public static List<Score> CreateMockScores()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), 1),
            new(Guid.NewGuid(), Guid.NewGuid(), 2),
            new(Guid.NewGuid(), Guid.NewGuid(), 3)
        ];
    }

    public static List<Report> CreateMockReports()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "text1"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "text2"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "text3")
        ];
    }

    public void Dispose() => Context.Dispose();
}