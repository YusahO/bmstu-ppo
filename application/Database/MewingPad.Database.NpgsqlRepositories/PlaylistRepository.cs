using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Utils.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class PlaylistRepository(MewingPadDbContext context) : IPlaylistRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddAudiofileToPlaylist(Guid playlistId, Guid audiofileId)
    {
        bool hasAudiofile = await _context.PlaylistsAudiofiles
            .AnyAsync(pa => pa.PlaylistId == playlistId && pa.AudiofileId == audiofileId);

        if (hasAudiofile)
        {
            await _context.PlaylistsAudiofiles
                .AddAsync(new(playlistId, audiofileId));
        }
    }

    public async Task AddPlaylist(Playlist playlist)
    {
        await _context.Playlists.AddAsync(PlaylistConverter.CoreToDbModel(playlist));
        await _context.SaveChangesAsync();
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        _context.Playlists.Remove(playlistDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Audiofile>> GetAllAudiofilesFromPlaylist(Guid playlistId)
    {
        var audiofiles = await _context.PlaylistsAudiofiles
            .Where(pa => pa.PlaylistId == playlistId)
            .Join(
                _context.Audiofiles,
                pa => pa.AudiofileId,
                a => a.Id,
                (pa, a) => a)
            .Select(a => AudiofileConverter.DbToCoreModel(a))
            .ToListAsync();
        return audiofiles;
    }

    public async Task<List<Playlist>> GetAllPlaylists()
    {
        return await _context.Playlists
            .Select(p => PlaylistConverter.DbToCoreModel(p)!)
            .ToListAsync();
    }

    public async Task<Playlist> GetPlaylistById(Guid playlistId)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        return PlaylistConverter.DbToCoreModel(playlistDbModel);
    }

    public async Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId)
    {
        var audiofileDbModel = await _context.PlaylistsAudiofiles
            .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId && pa.AudiofileId == audiofileId);

        if (audiofileDbModel is not null)
        {
            _context.PlaylistsAudiofiles.Remove(audiofileDbModel);
        }
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAudiofilesFromPlaylistBulk(Guid playlistId, List<Guid> audiofileIds)
    {
        foreach (var audiofileId in audiofileIds)
        {
            var audiofileDbModel = await _context.PlaylistsAudiofiles
                .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId && pa.AudiofileId == audiofileId);

            if (audiofileDbModel is not null)
            {
                _context.PlaylistsAudiofiles.Remove(audiofileDbModel);
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task<Playlist> UpdatePlaylist(Playlist playlist)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlist.Id);

        playlistDbModel!.Id = playlist.Id;
        playlistDbModel!.Title = playlist.Title;
        playlistDbModel!.UserId = playlist.UserId;

        await _context.SaveChangesAsync();
        return PlaylistConverter.DbToCoreModel(playlistDbModel);
    }
}