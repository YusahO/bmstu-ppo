using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface IPlaylistRepository
{
    Task AddPlaylist(Playlist playlist);
    Task<Playlist> UpdatePlaylist(Playlist playlist);
    Task DeletePlaylist(Guid playlistId);
    Task<List<Playlist>> GetUserPlaylists(Guid userId);
    Task<bool> IsAudiotrackInPlaylist(Guid playlistId, Guid audiotrackId);
    Task<List<Audiotrack>> GetAllAudiotracksFromPlaylist(Guid playlistId);
    Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiotrackId);
    Task RemoveAudiofilesFromPlaylistBulk(Guid playlistId, List<Guid> audiotrackIds);
    Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId);
    Task<List<Playlist>> GetAllPlaylists();
    Task<Playlist?> GetPlaylistById(Guid playlistId);
}