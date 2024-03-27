using MewingPad.Common.IRepositories;
using MewingPad.Common.Entities;
using MewingPad.Database.Context;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Services.DbFixtures;

public class InMemoryDbFixture : IDisposable
{
    private readonly MewingPadDbContext _context = new InMemoryDbContextFactory().GetDbContext();

    public IUserRepository UserRepository { get; }
    public IPlaylistRepository PlaylistRepository { get; }
    public IAudiofileRepository AudiofileRepository { get; }
    public ITagRepository TagRepository { get; }
    public IScoreRepository ScoreRepository { get; }
    public ICommentaryRepository CommentaryRepository { get; }
    public IReportRepository ReportRepository { get; }

    public InMemoryDbFixture()
    {
        UserRepository = new UserRepository(_context);
        PlaylistRepository = new PlaylistRepository(_context);
        AudiofileRepository = new AudiofileRepository(_context);
        TagRepository = new TagRepository(_context);
        ScoreRepository = new ScoreRepository(_context);
        CommentaryRepository = new CommentaryRepository(_context);
        ReportRepository = new ReportRepository(_context);
    }

    public async Task InsertUsers(List<User> users)
    {
        foreach (var user in users)
        {
            await UserRepository.AddUser(user);
        }
    }

    public async Task InsertPlaylists(List<Playlist> playlists)
    {
        foreach (var playlist in playlists)
        {
            await PlaylistRepository.AddPlaylist(playlist);
        }
    }

    public async Task InsertAudiofiles(List<Audiofile> audiofiles)
    {
        foreach (var audiofile in audiofiles)
        {
            await AudiofileRepository.AddAudiofile(audiofile);
        }
    }

    public async Task InsertTags(List<Tag> tags)
    {
        foreach (var tag in tags)
        {
            await TagRepository.AddTag(tag);
        }
    }

    public async Task InsertCommentaries(List<Commentary> commentaries)
    {
        foreach (var commentary in commentaries)
        {
            await CommentaryRepository.AddCommentary(commentary);
        }
    }

    public async Task InsertPlaylistsAudiofiles(List<KeyValuePair<Guid, Guid>> pairs)
    {
        foreach (var p in pairs)
        {
            await _context.PlaylistsAudiofiles.AddAsync(new(p.Key, p.Value));
        }
        await _context.SaveChangesAsync();
    }

    public async Task InsertTagsAudiofiles(List<KeyValuePair<Guid, Guid>> pairs)
    {
        foreach (var p in pairs)
        {
            await _context.TagsAudiofiles.AddAsync(new(p.Key, p.Value));
        }
        await _context.SaveChangesAsync();
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
            new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1"),
            new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2"),
            new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "3")
        ];
    }

    public void Dispose() => _context.Dispose();
}