using MewingPad.Services.PlaylistService;
using IntegrationTests.Services.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Services.IntegratonTests;

public class PlaylistServiceIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly IPlaylistService _playlistService;

    public PlaylistServiceIntegrationTests()
    {
        _dbFixture = new();
        _playlistService = new PlaylistService(_dbFixture.PlaylistRepository,
                                               _dbFixture.AudiofileRepository,
                                               _dbFixture.UserRepository);
    }

    [Fact]
    public async Task TestGetPlaylistById()
    {
        const int ind = 0;
        var expectedPlaylists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(expectedPlaylists);

        var actualPlaylist = await _playlistService.GetPlaylistById(expectedPlaylists[ind].Id);

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

        var actualAudiofiles = await _playlistService.GetAllAudiofilesFromPlaylist(playlist.Id);

        Assert.Equal(expectedAudiofiles[..2], actualAudiofiles);
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromEmptyPlaylist()
    {
        var playlist = new Playlist(Guid.NewGuid(), "title", Guid.NewGuid());
        await _dbFixture.InsertPlaylists([playlist]);

        var actualAudiofiles = await _playlistService.GetAllAudiofilesFromPlaylist(playlist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestGetUserFavouritesPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        var users = InMemoryDbFixture.CreateMockUsers();

        var expectedUser = users.First();
        var expectedPlaylist = playlists.First();
        expectedUser.FavouritesId = expectedPlaylist.Id;

        await _dbFixture.InsertUsers(users);
        await _dbFixture.InsertPlaylists(playlists);

        var actualPlaylist = await _playlistService.GetUserFavouritesPlaylist(expectedUser.Id);

        Assert.Equal(expectedPlaylist, actualPlaylist);
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

        await _playlistService.AddAudiofileToPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _playlistService.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

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

        await _playlistService.RemoveAudiofileFromPlaylist(expectedPlaylist.Id, expectedAudiofile.Id);

        var actualAudiofiles = await _playlistService.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

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

        await _playlistService.RemoveAudiofilesFromPlaylist(expectedPlaylist.Id, expectedAudiofileIds);

        var actualAudiofiles = await _playlistService.GetAllAudiofilesFromPlaylist(expectedPlaylist.Id);

        Assert.Empty(actualAudiofiles);
    }

    [Fact]
    public async Task TestRemoveAudiofileFromPlaylistNonexistent()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        Task Action() => _playlistService.RemoveAudiofileFromPlaylist(Guid.Empty, audiofiles.First().Id);
        
        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    }

    [Fact]
    public async Task TestRemoveAudiofileNonexistentFromPlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(playlists);

        Task Action() => _playlistService.RemoveAudiofileFromPlaylist(playlists.First().Id, Guid.Empty);
        
        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeletePlaylist()
    {
        var playlists = InMemoryDbFixture.CreateMockPlaylists();
        await _dbFixture.InsertPlaylists(playlists);

        var expectedPlaylistId = playlists.First().Id;
        await _playlistService.DeletePlaylist(expectedPlaylistId);

        Task Action() => _playlistService.GetPlaylistById(expectedPlaylistId);
        
        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    }

    public void Dispose() => _dbFixture.Dispose();
}