using MewingPad.Services.PlaylistService;
using IntegrationTests.Services.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Services.IntegratonTests;

public class PlaylistIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly IPlaylistService _playlistService;

    public PlaylistIntegrationTests()
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

    public void Dispose() =>  _dbFixture.Dispose();
}