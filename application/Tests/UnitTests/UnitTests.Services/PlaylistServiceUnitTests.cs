using MewingPad.Common.Exceptions;
using MewingPad.Services.PlaylistService;

namespace UnitTests.Services;

public class PlaylistServiceUnitTest
{
    private readonly IPlaylistService _playlistService;
    private readonly Mock<IPlaylistRepository> _mockPlaylistRepository = new();
    private readonly Mock<IAudiotrackRepository> _mockAudiotrackRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public PlaylistServiceUnitTest() => _playlistService = new PlaylistService(_mockPlaylistRepository.Object,
                                                                               _mockAudiotrackRepository.Object,
                                                                               _mockUserRepository.Object);

    [Fact]
    public async Task TestCreatePlaylist()
    {
        List<Playlist> playlists = [];
        var expectedPlaylist = CreateMockPlaylist();

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(expectedPlaylist.Id))
                               .ReturnsAsync(default(Playlist)!);

        _mockPlaylistRepository.Setup(s => s.AddPlaylist(It.IsAny<Playlist>()))
                               .Callback((Playlist p) => playlists.Add(p));

        await _playlistService.CreatePlaylist(expectedPlaylist);
        var actualPlaylist = playlists.Last();

        Assert.Equal(expectedPlaylist, actualPlaylist);
    }

    [Fact]
    public async Task TestCreateExistentPlaylist()
    {
        var expectedPlaylist = CreateMockPlaylist();

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(expectedPlaylist.Id))
                               .ReturnsAsync(expectedPlaylist);

        async Task Action() => await _playlistService.CreatePlaylist(expectedPlaylist);

        await Assert.ThrowsAsync<PlaylistExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdatePlaylistTitle()
    {
        var expectedPlaylist = CreateMockPlaylist();

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(expectedPlaylist.Id))
                               .ReturnsAsync(expectedPlaylist);

        _mockPlaylistRepository.Setup(s => s.UpdatePlaylist(expectedPlaylist))
                               .ReturnsAsync(expectedPlaylist);

        var actualPlaylist = await _playlistService.UpdateTitle(expectedPlaylist.Id, expectedPlaylist.Title);

        Assert.Equal(expectedPlaylist.Title, actualPlaylist.Title);
    }

    [Fact]
    public async Task TestUpdatePlaylistNonexistent()
    {
        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
                               .ReturnsAsync(default(Playlist)!);

        async Task Action() => await _playlistService.UpdateTitle(Guid.Empty, "new title");

        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeletePlaylist()
    {
        List<Playlist> playlists = [CreateMockPlaylist()];
        var expectedPlaylist = new Playlist(playlists.First());

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(expectedPlaylist.Id))
                               .ReturnsAsync(expectedPlaylist);

        _mockPlaylistRepository.Setup(s => s.DeletePlaylist(It.IsAny<Guid>()))
                               .Callback((Guid id) =>
                               {
                                   playlists.Remove(expectedPlaylist);
                               });

        await _playlistService.DeletePlaylist(expectedPlaylist.Id);

        Assert.Empty(playlists);
    }

    [Fact]
    public async Task TestDeletePlaylistNonexistent()
    {
        var expectedPlaylist = CreateMockPlaylist();

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(expectedPlaylist.Id))
                               .ReturnsAsync(default(Playlist)!);

        async Task Action() => await _playlistService.DeletePlaylist(expectedPlaylist.Id);

        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetUserFavouritesPlaylist()
    {
        var expectedPlaylist = CreateMockPlaylist();
        var user = new User(Guid.NewGuid(), expectedPlaylist.Id, "username", "email", "", false);

        _mockUserRepository.Setup(s => s.GetUserById(user.Id))
                           .ReturnsAsync(user);
        
        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(user.FavouritesId))
                               .ReturnsAsync(expectedPlaylist);

        var actualPlaylist = await _playlistService.GetUserFavouritesPlaylist(user.Id);

        Assert.Equal(user.FavouritesId, actualPlaylist.Id);
        Assert.Equal(expectedPlaylist, actualPlaylist);
    }

    [Fact]
    public async Task TestGetUserNonexistentFavouritesPlaylist()
    {
        var expectedPlaylist = CreateMockPlaylist();

        _mockUserRepository.Setup(s => s.GetUserById(It.IsAny<Guid>()))
                           .ReturnsAsync(default(User)!);

        async Task Action() => await _playlistService.GetUserFavouritesPlaylist(Guid.Empty);

        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromPlaylist()
    {
        var playlist = CreateMockPlaylist();
        var expectedFiles = CreateMockAudiofiles();

        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
                               .ReturnsAsync(playlist);

        _mockPlaylistRepository.Setup(s => s.GetAllAudiotracksFromPlaylist(playlist.Id))
                               .ReturnsAsync(expectedFiles);

        var actualFiles = await _playlistService.GetAllAudiotracksFromPlaylist(playlist.Id);

        Assert.Equal(expectedFiles, actualFiles);  
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromPlaylistNonexistent()
    {
        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
                               .ReturnsAsync(default(Playlist)!);

        async Task Action() => await _playlistService.GetAllAudiotracksFromPlaylist(Guid.Empty);

        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);  
    }

    private static Playlist CreateMockPlaylist()
    {
        return new(Guid.NewGuid(), "mock title", Guid.NewGuid());
    }

    private static List<Audiotrack> CreateMockAudiofiles()
    {
        return
        [
            new(Guid.NewGuid(), "title1", 5.43f, Guid.NewGuid(), "path/to/file1"),
            new(Guid.NewGuid(), "title2", 2.00f, Guid.NewGuid(), "path/to/file2"),
            new(Guid.NewGuid(), "title3", 10.03f, Guid.NewGuid(), "path/to/file3"),
        ];
    }
}