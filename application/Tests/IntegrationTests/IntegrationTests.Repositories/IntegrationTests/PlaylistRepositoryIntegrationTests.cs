using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class PlaylistRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistRepositoryIntegrationTests()
    {
        _playlistRepository = new PlaylistRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreatePlaylist()
    {
        var expectedPlaylist = new Playlist(Guid.NewGuid(), "title", Guid.Empty);
        
        await _playlistRepository.AddPlaylist(expectedPlaylist);
        var actualPlaylist = await _dbFixture.GetPlaylistById(expectedPlaylist.Id);

        Assert.Equal(expectedPlaylist, actualPlaylist);
    }

    [Fact]
    public async Task TestGetPlaylistById()
    {
        const int ind = 0;
        var expectedPlaylists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(expectedPlaylists);

        var actualPlaylist = await _playlistRepository.GetPlaylistById(expectedPlaylists[ind].Id);

        Assert.Equal(expectedPlaylists[ind], actualPlaylist);
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromPlaylist()
    {
        var playlist = new Playlist(Guid.NewGuid(), "title", Guid.NewGuid());
        var expectedAudiofiles = InMemoryDbFixture.CreateMockAudiotracks();

        List<KeyValuePair<Guid, Guid>> pairs = [
            new(playlist.Id, expectedAudiofiles[0].Id),
            new(playlist.Id, expectedAudiofiles[1].Id)
        ];

        await _dbFixture.InsertPlaylists([playlist]);
        await _dbFixture.InsertAudiotracks(expectedAudiofiles);
        await _dbFixture.InsertPlaylistsAudiotracks(pairs);

        var actualAudiofiles = await _playlistRepository.GetAllAudiotracksFromPlaylist(playlist.Id);

        Assert.Equal(expectedAudiofiles[..2].OrderBy(e => e.Id), actualAudiofiles.OrderBy(a => a.Id));
    }

    [Fact]
    public async Task TestAddAudiofileToPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();
        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiotracks(audiofiles);

        var expectedPlaylist = playlists.First();
        var expectedAudiofile = audiofiles.First();

        await _playlistRepository.AddAudiotrackToPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _dbFixture.GetAllAudiotracksFromPlaylist(expectedPlaylist.Id);

        Assert.Equal([expectedAudiofile], actualAudiofiles);
    }

    [Fact]
    public async Task TestRemoveAudiofileFromPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();
        var expectedPlaylist = playlists.First();
        var expectedAudiofile = audiofiles.First();

        List<KeyValuePair<Guid, Guid>> pairs = [
            new(expectedPlaylist.Id, expectedAudiofile.Id)];

        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiotracks(audiofiles);
        await _dbFixture.InsertPlaylistsAudiotracks(pairs);

        await _playlistRepository.RemoveAudiotrackFromPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _dbFixture.GetAllAudiotracksFromPlaylist(expectedPlaylist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestRemoveAudiofilesFromPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();

        var expectedPlaylist = playlists.First();
        var expectedAudiofileIds = audiofiles[..2].Select(a => a.Id)
                                                .ToList();
        List<KeyValuePair<Guid, Guid>> pairs = expectedAudiofileIds!
            .Select(aid => new KeyValuePair<Guid, Guid>(expectedPlaylist.Id, aid))
            .ToList();

        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiotracks(audiofiles);
        await _dbFixture.InsertPlaylistsAudiotracks(pairs);

        await _playlistRepository.RemoveAudiotracksFromPlaylistBulk(expectedPlaylist.Id, expectedAudiofileIds);

        var actualAudiofiles = await _dbFixture.GetAllAudiotracksFromPlaylist(expectedPlaylist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestDeletePlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(playlists);

        var expectedPlaylistId = playlists.First().Id;
        await _playlistRepository.DeletePlaylist(expectedPlaylistId);

        var actualPlaylist = await _dbFixture.GetPlaylistById(expectedPlaylistId);
        
        Assert.Null(actualPlaylist);
    }

    public void Dispose() => _dbFixture.Dispose();
}