using MewingPad.Common.Entities;
namespace MewingPad.Services.PlaylistService;

public interface IPlaylistService
{
    Task CreatePlaylist(Playlist playlist);
    Task<Playlist> UpdateTitle(Guid playlistId, string? title);
    Task DeletePlaylist(Guid playlistId);
    Task<Playlist> GetUserFavouritesPlaylist(Guid userId);
    Task<List<Audiofile>> GetAllAudiofilesFromPlaylist(Guid playlistId);
    Task AddAudiofileToPlaylist(Guid playlistId, Guid audiofileId);
    Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId);
    Task RemoveAudiofilesFromPlaylist(Guid playlistId, List<Guid> audiofileIds);
}
