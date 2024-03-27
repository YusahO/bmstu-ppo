using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.PlaylistService;

public class PlaylistService(IPlaylistRepository playlistRepository,
                             IAudiofileRepository audiofileRepository,
                             IUserRepository userRepository) : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository = playlistRepository
                                                               ?? throw new ArgumentNullException();
    private readonly IAudiofileRepository _audiofileRepository = audiofileRepository
                                                                 ?? throw new ArgumentNullException();
    private readonly IUserRepository _userRepository = userRepository
                                                       ?? throw new ArgumentNullException();

    public async Task CreatePlaylist(Playlist playlist)
    {
        if (await _playlistRepository.GetPlaylistById(playlist.Id) is not null)
        {
            throw new PlaylistExistsException(playlist.Id);
        }
        await _playlistRepository.AddPlaylist(playlist);
    }

    public async Task<Playlist> UpdateTitle(Guid playlistId, string title)
    {
        Playlist foundPlaylist = await _playlistRepository.GetPlaylistById(playlistId)
                                 ?? throw new PlaylistNotFoundException(playlistId);
        foundPlaylist.Title = title;
        return await _playlistRepository.UpdatePlaylist(foundPlaylist);
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            throw new PlaylistNotFoundException(playlistId);
        }
        await _playlistRepository.DeletePlaylist(playlistId);
    }

    public async Task AddAudiofileToPlaylist(Guid playlistId, Guid audiofileId)
    {
        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            throw new PlaylistNotFoundException(playlistId);
        }
        if (await _audiofileRepository.GetAudiofileById(audiofileId) is null)
        {
            throw new AudiofileNotFoundException(playlistId);
        }
        await _playlistRepository.AddAudiofileToPlaylist(playlistId, audiofileId);
    }

    public async Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId)
    {
        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            throw new PlaylistNotFoundException(playlistId);
        }
        if (await _audiofileRepository.GetAudiofileById(audiofileId) is null)
        {
            throw new AudiofileNotFoundException(audiofileId);
        }
        await _playlistRepository.RemoveAudiofileFromPlaylist(playlistId, audiofileId);
    }

    public async Task RemoveAudiofilesFromPlaylist(Guid playlistId, List<Guid> audiofileIds)
    {
        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            throw new PlaylistNotFoundException(playlistId);
        }
        await _playlistRepository.RemoveAudiofilesFromPlaylistBulk(playlistId, audiofileIds);
    }

    public async Task<Playlist> GetUserFavouritesPlaylist(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId)
                   ?? throw new UserNotFoundException(userId);
        return await _playlistRepository.GetPlaylistById(user.FavouritesId);
    }

    public async Task<List<Audiofile>> GetAllAudiofilesFromPlaylist(Guid playlistId)
    {
        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            throw new PlaylistNotFoundException(playlistId);
        }
        return await _playlistRepository.GetAllAudiofilesFromPlaylist(playlistId);
    }

    public async Task<Playlist> GetPlaylistById(Guid playlistId)
    {
        var playlist = await _playlistRepository.GetPlaylistById(playlistId)
                              ?? throw new PlaylistNotFoundException(playlistId);
        return playlist;
    }
}