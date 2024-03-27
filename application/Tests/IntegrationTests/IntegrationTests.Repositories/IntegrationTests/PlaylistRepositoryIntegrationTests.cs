using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class PlaylistRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreatePlaylist()
    {
        var expectedPlaylist = new Playlist(Guid.NewGuid(), "title", Guid.Empty);
        
        await _dbFixture.PlaylistRepository.AddPlaylist(expectedPlaylist);
        var actualPlaylist = await _dbFixture.PlaylistRepository.GetPlaylistById(expectedPlaylist.Id);

        Assert.Equal(expectedPlaylist, actualPlaylist);
    }

    [Fact]
    public async Task TestGetPlaylistById()
    {
        const int ind = 0;
        var expectedPlaylists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(expectedPlaylists);

        var actualPlaylist = await _dbFixture.PlaylistRepository.GetPlaylistById(expectedPlaylists[ind].Id);

        Assert.Equal(expectedPlaylists[ind], actualPlaylist);
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromPlaylist()
    {
        var playlist = new Playlist(Guid.NewGuid(), "title", Guid.NewGuid());
        var expectedAudiofiles = InMemoryDbFixture.CreateMockAudiofiles();

        List<KeyValuePair<Guid, Guid>> pairs = [
            new(playlist.Id, expectedAudiofiles[0].Id),
            new(playlist.Id, expectedAudiofiles[1].Id)
        ];

        await _dbFixture.InsertPlaylists([playlist]);
        await _dbFixture.InsertAudiofiles(expectedAudiofiles);
        await _dbFixture.InsertPlaylistsAudiofiles(pairs);

        var actualAudiofiles = await _dbFixture.PlaylistRepository.GetAllAudiofilesFromPlaylist(playlist.Id);

        Assert.Equal(expectedAudiofiles[..2], actualAudiofiles);
    }

    [Fact]
    public async Task TestAddAudiofileToPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedPlaylist = playlists.First();
        var expectedAudiofile = audiofiles.First();

        await _dbFixture.PlaylistRepository.AddAudiofileToPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _dbFixture.PlaylistRepository.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

        Assert.Equal([expectedAudiofile], actualAudiofiles);
    }

    [Fact]
    public async Task TestRemoveAudiofileFromPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        var expectedPlaylist = playlists.First();
        var expectedAudiofile = audiofiles.First();

        List<KeyValuePair<Guid, Guid>> pairs = [
            new(expectedPlaylist.Id, expectedAudiofile.Id)];

        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiofiles(audiofiles);
        await _dbFixture.InsertPlaylistsAudiofiles(pairs);

        await _dbFixture.PlaylistRepository.RemoveAudiofileFromPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _dbFixture.PlaylistRepository.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestRemoveAudiofilesFromPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();

        var expectedPlaylist = playlists.First();
        var expectedAudiofileIds = audiofiles[..2].Select(a => a.Id)
                                                .ToList();
        List<KeyValuePair<Guid, Guid>> pairs = expectedAudiofileIds!
            .Select(aid => new KeyValuePair<Guid, Guid>(expectedPlaylist.Id, aid))
            .ToList();

        await _dbFixture.InsertPlaylists(playlists);
        await _dbFixture.InsertAudiofiles(audiofiles);
        await _dbFixture.InsertPlaylistsAudiofiles(pairs);

        await _dbFixture.PlaylistRepository.RemoveAudiofilesFromPlaylistBulk(expectedPlaylist.Id, expectedAudiofileIds);

        var actualAudiofiles = await _dbFixture.PlaylistRepository.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestDeletePlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(playlists);

        var expectedPlaylistId = playlists.First().Id;
        await _dbFixture.PlaylistRepository.DeletePlaylist(expectedPlaylistId);

        var actualPlaylist = await _dbFixture.PlaylistRepository.GetPlaylistById(expectedPlaylistId);
        
        Assert.Null(actualPlaylist);
    }

    public void Dispose() => _dbFixture.Dispose();
}