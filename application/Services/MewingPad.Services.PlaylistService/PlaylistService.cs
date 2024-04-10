using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using Serilog;

namespace MewingPad.Services.PlaylistService;

public class PlaylistService(IPlaylistRepository playlistRepository,
                             IAudiotrackRepository audiofileRepository,
                             IUserRepository userRepository,
                             IPlaylistAudiotrackRepository playlistAudiotrackRepository) : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository = playlistRepository
                                                               ?? throw new ArgumentNullException();
    private readonly IAudiotrackRepository _audiotrackRepository = audiofileRepository
                                                                 ?? throw new ArgumentNullException();
    private readonly IUserRepository _userRepository = userRepository
                                                       ?? throw new ArgumentNullException();
    private readonly IPlaylistAudiotrackRepository _playlistAudiotrackRepository = playlistAudiotrackRepository
                                                                                   ?? throw new ArgumentNullException();

    private readonly ILogger _logger = Log.ForContext<PlaylistService>();

    public async Task CreatePlaylist(Playlist playlist)
    {
        _logger.Information("Entering CreatePlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlist.Id) is not null)
        {
            throw new PlaylistExistsException(playlist.Id);
        }
        await _playlistRepository.AddPlaylist(playlist);
        _logger.Information("Exiting CreatePlaylist method");
    }

    public async Task<Playlist> UpdatePlaylistTitle(Guid playlistId, string title)
    {
        _logger.Information("Entering UpdatePlaylistTitle method");

        var playlist = await _playlistRepository.GetPlaylistById(playlistId);
        if (playlist is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        playlist.Title = title;

        await _playlistRepository.UpdatePlaylist(playlist);
        _logger.Information("Exiting UpdatePlaylistTitle method");
        return playlist;
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        _logger.Information("Entering DeletePlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }

        await _playlistAudiotrackRepository.DeleteByPlaylist(playlistId);
        await _playlistRepository.DeletePlaylist(playlistId);

        _logger.Information("Exiting DeletePlaylist method");
    }

    public async Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Information("Entering AddAudiotrackToPlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        if (await _audiotrackRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }

        if (await _playlistAudiotrackRepository.IsAudiotrackInPlaylist(playlistId, audiotrackId))
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) is already in playlist (Id = {playlistId})");
            throw new AudiotrackExistsInPlaylistException(playlistId, audiotrackId);
        }
        await _playlistAudiotrackRepository.AddAudiotrackToPlaylist(playlistId, audiotrackId);


        _logger.Information("Exiting AddAudiotrackToPlaylist method");
    }

    public async Task RemoveAudiotrackFromPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Information("Entering RemoveAudiotrackFromPlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        if (await _audiotrackRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        if (!await _playlistAudiotrackRepository.IsAudiotrackInPlaylist(playlistId, audiotrackId))
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) is not in playlist (Id = {playlistId})");
            throw new AudiotrackNotFoundInPlaylistException(playlistId, audiotrackId);
        }
        await _playlistAudiotrackRepository.RemoveAudiotrackFromPlaylist(playlistId, audiotrackId);

        _logger.Information("Exiting RemoveAudiotrackFromPlaylist method");
    }

    public async Task RemoveAudiotracksFromPlaylist(Guid playlistId, List<Guid> audiotrackIds)
    {
        _logger.Information("Entering RemoveAudiotracksFromPlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        foreach (var aid in audiotrackIds)
        {
            if (await _audiotrackRepository.GetAudiotrackById(aid) is null)
            {
                _logger.Error($"Audiotrack (Id = {aid}) not found");
                throw new AudiotrackNotFoundException(aid);
            }
            if (!await _playlistAudiotrackRepository.IsAudiotrackInPlaylist(playlistId, aid))
            {
                _logger.Error($"Audiotrack (Id = {aid}) is not in playlist (Id = {playlistId})");
                throw new AudiotrackNotFoundInPlaylistException(playlistId, aid);
            }
        }
        await _playlistAudiotrackRepository.RemoveAudiotracksFromPlaylist(playlistId, audiotrackIds);
        _logger.Information("Exiting RemoveAudiotracksFromPlaylist method");
    }

    public async Task<Playlist> GetUserFavouritesPlaylist(Guid userId)
    {
        _logger.Information("Entering GetUserFavouritesPlaylist method");

        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            _logger.Error($"User (Id = {userId}) not found");
            throw new UserNotFoundException(userId);
        }

        var playlist = await _playlistRepository.GetPlaylistById(user.FavouritesId);
        if (playlist is null)
        {
            _logger.Error($"Favourites playlist (Id = {user.FavouritesId}) not found");
            throw new PlaylistNotFoundException(user.FavouritesId);
        }
        _logger.Information("Exiting GetUserFavouritesPlaylist method");
        return playlist!;
    }

    public async Task<List<Audiotrack>> GetAllAudiotracksFromPlaylist(Guid playlistId)
    {
        _logger.Information("Entering GetAllAudiotracksFromPlaylist method");

        if (await _playlistRepository.GetPlaylistById(playlistId) is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        var audios = await _playlistAudiotrackRepository.GetAllAudiotracksFromPlaylist(playlistId);
        _logger.Information("Exiting GetAllAudiotracksFromPlaylist method");
        return audios;
    }

    public async Task<Playlist> GetPlaylistById(Guid playlistId)
    {
        _logger.Information("Entering GetPlaylistById method");

        var playlist = await _playlistRepository.GetPlaylistById(playlistId);
        if (playlist is null)
        {
            _logger.Error($"Playlist (Id = {playlistId}) not found");
            throw new PlaylistNotFoundException(playlistId);
        }
        _logger.Information("Exiting GetPlaylistById method");
        return playlist;
    }

    public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
    {
        _logger.Information("Entering GetUserPlaylists method");

        if (await _userRepository.GetUserById(userId) is null)
        {
            _logger.Error($"User (Id = {userId}) not found");
            throw new UserNotFoundException(userId);
        }
        var playlists = await _playlistRepository.GetUserPlaylists(userId);

        _logger.Information("Exiting GetUserPlaylists method");
        return playlists;
    }
}