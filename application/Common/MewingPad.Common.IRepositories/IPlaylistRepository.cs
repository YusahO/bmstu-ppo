using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface IPlaylistRepository
{
    Task AddPlaylist(Playlist playlist);
    Task<Playlist> UpdatePlaylist(Playlist playlist);
    Task DeletePlaylist(Guid playlistId);
    Task<List<Audiofile>> GetAllAudiofilesFromPlaylist(Guid playlistId);
    Task AddAudiofileToPlaylist(Guid playlistId, Guid audiofileId);
    Task RemoveAudiofilesFromPlaylistBulk(Guid playlistId, List<Guid> audiofileIds);
    Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId);
    Task<List<Playlist>> GetAllPlaylists();
    Task<Playlist> GetPlaylistById(Guid playlistId);
}