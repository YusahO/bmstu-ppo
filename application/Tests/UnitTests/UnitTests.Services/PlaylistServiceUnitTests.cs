using MewingPad.Common.Exceptions;
using MewingPad.Services.PlaylistService;

namespace UnitTests.Services;

public class PlaylistServiceUnitTest
{
    private readonly IPlaylistService _playlistService;
    private readonly Mock<IPlaylistRepository> _mockPlaylistRepository = new();
    private readonly Mock<IAudiofileRepository> _mockAudiofileRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public PlaylistServiceUnitTest() => _playlistService = new PlaylistService(_mockPlaylistRepository.Object,
                                                                               _mockAudiofileRepository.Object,
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
        var user = new User(Guid.NewGuid(), expectedPlaylist.Id, "username", "email", "", false, false);

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

        _mockPlaylistRepository.Setup(s => s.GetAllAudiofilesFromPlaylist(playlist.Id))
                               .ReturnsAsync(expectedFiles);

        var actualFiles = await _playlistService.GetAllAudiofilesFromPlaylist(playlist.Id);

        Assert.Equal(expectedFiles, actualFiles);  
    }

    [Fact]
    public async Task TestGetAllAudiofilesFromPlaylistNonexistent()
    {
        _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
                               .ReturnsAsync(default(Playlist)!);

        async Task Action() => await _playlistService.GetAllAudiofilesFromPlaylist(Guid.Empty);

        await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);  
    }

    // [Fact]
    // public async Task TestAddAudiofile()
    // {
    //     var playlist = CreateMockPlaylist();
    //     var file = CreateMockAudiofile();

    //     KeyValuePair<Guid, Guid> connection = new();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
    //                            .ReturnsAsync(playlist);

    //     _mockAudiofileRepository.Setup(s => s.GetAudiofileById(file.Id))
    //                             .ReturnsAsync(file);

    //     _mockPlaylistRepository.Setup(s => s.AddAudiofileToPlaylist(playlist.Id, file.Id))
    //                            .Callback((Guid k, Guid v) =>
    //                            {
    //                                connection = new KeyValuePair<Guid, Guid>(k, v);
    //                            });

    //     await _playlistService.AddAudiofileToPlaylist(playlist.Id, file.Id);

    //     Assert.Equal(connection.Key, playlist.Id);
    //     Assert.Equal(connection.Value, file.Id);
    // }

    // [Fact]
    // public async Task TestAddAudiofileNonexistent()
    // {
    //     var playlist = CreateMockPlaylist();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
    //                            .ReturnsAsync(playlist);

    //     _mockAudiofileRepository.Setup(s => s.GetAudiofileById(It.IsAny<Guid>()))
    //                             .ReturnsAsync(default(Audiofile)!);

    //     async Task Action() => await _playlistService.AddAudiofileToPlaylist(playlist.Id, Guid.NewGuid());

    //     await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    // }

    // [Fact]
    // public async Task TestAddAudiofileToNonexistentPlaylist()
    // {
    //     var file = CreateMockAudiofile();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
    //                            .ReturnsAsync(default(Playlist)!);

    //     _mockAudiofileRepository.Setup(s => s.GetAudiofileById(file.Id))
    //                             .ReturnsAsync(file);

    //     async Task Action() => await _playlistService.AddAudiofileToPlaylist(Guid.NewGuid(), file.Id);

    //     await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    // }

    // [Fact]
    // public async Task TestRemoveAudiofileFromPlaylist()
    // {
    //     var playlist = CreateMockPlaylist();
    //     var file = CreateMockAudiofile();

    //     List<KeyValuePair<Guid, Guid>> connections = [new(playlist.Id, file.Id)];

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
    //                            .ReturnsAsync(playlist);

    //     _mockAudiofileRepository.Setup(s => s.GetAudiofileById(file.Id))
    //                             .ReturnsAsync(file);

    //     _mockPlaylistRepository.Setup(s => s.RemoveAudiofileFromPlaylist(playlist.Id, file.Id))
    //                            .Callback((Guid k, Guid v) =>
    //                            {
    //                                connections.RemoveAt(0);
    //                            });

    //     await _playlistService.RemoveAudiofileFromPlaylist(playlist.Id, file.Id);

    //     Assert.Empty(connections);
    // }

    // [Fact]
    // public async Task TestRemoveAudiofileFromPlaylistNonexistent()
    // {
    //     var file = CreateMockAudiofile();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
    //                            .ReturnsAsync(default(Playlist)!);

    //     async Task Action() => await _playlistService.RemoveAudiofileFromPlaylist(Guid.Empty, file.Id);

    //     await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    // }

    // [Fact]
    // public async Task TestRemoveAudiofileNonexistentFromPlaylist()
    // {
    //     var playlist = CreateMockPlaylist();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
    //                            .ReturnsAsync(playlist);
    //     _mockAudiofileRepository.Setup(s => s.GetAudiofileById(It.IsAny<Guid>()))
    //                             .ReturnsAsync(default(Audiofile)!);

    //     async Task Action() => await _playlistService.RemoveAudiofileFromPlaylist(playlist.Id, Guid.Empty);

    //     await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    // }

    // [Fact]
    // public async Task TestRemoveAudiofilesFromPlaylist()
    // {
    //     var playlist = CreateMockPlaylist();
    //     var files = CreateMockAudiofiles();
    //     var fileIds = files.Select(s => s.Id).ToList();

    //     List<KeyValuePair<Guid, Guid>> connections = [];
    //     foreach (var fid in fileIds)
    //     {
    //         connections.Add(new(playlist.Id, fid));
    //     }

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(playlist.Id))
    //                            .ReturnsAsync(playlist);

    //     _mockPlaylistRepository.Setup(s => s.RemoveAudiofilesFromPlaylistBulk(playlist.Id, fileIds))
    //                            .Callback((Guid p, List<Guid> f) =>
    //                            {
    //                                connections.Clear();
    //                            });

    //     await _playlistService.RemoveAudiofilesFromPlaylist(playlist.Id, fileIds);

    //     Assert.Empty(connections);
    // }

    // [Fact]
    // public async Task TestRemoveAudiofilesFromPlaylistNonexistent()
    // {
    //     var files = CreateMockAudiofiles();
    //     var fileIds = files.Select(s => s.Id).ToList();

    //     _mockPlaylistRepository.Setup(s => s.GetPlaylistById(It.IsAny<Guid>()))
    //                            .ReturnsAsync(default(Playlist)!);

    //     async Task Action() => await _playlistService.RemoveAudiofilesFromPlaylist(Guid.NewGuid(), fileIds);

    //     await Assert.ThrowsAsync<PlaylistNotFoundException>(Action);
    // }

    private static Playlist CreateMockPlaylist()
    {
        return new(Guid.NewGuid(), "mock title", Guid.NewGuid());
    }

    private static List<Audiofile> CreateMockAudiofiles()
    {
        return
        [
            new(Guid.NewGuid(), "title1", 5.43f, Guid.NewGuid(), "path/to/file1"),
            new(Guid.NewGuid(), "title2", 2.00f, Guid.NewGuid(), "path/to/file2"),
            new(Guid.NewGuid(), "title3", 10.03f, Guid.NewGuid(), "path/to/file3"),
        ];
    }
}