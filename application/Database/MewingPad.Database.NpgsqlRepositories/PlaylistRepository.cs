using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class PlaylistRepository(MewingPadDbContext context) : IPlaylistRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiofileId)
    {
        bool hasAudiofile = await _context.PlaylistsAudiotracks
            .AnyAsync(pa => pa.PlaylistId == playlistId && pa.AudiotrackId == audiofileId);

        if (!hasAudiofile)
        {
            await _context.PlaylistsAudiotracks
                .AddAsync(new(playlistId, audiofileId));
        }
        await _context.SaveChangesAsync();
    }

    public async Task AddPlaylist(Playlist playlist)
    {
        var playlistDbModel = PlaylistConverter.CoreToDbModel(playlist);
        await _context.Playlists.AddAsync(playlistDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAudiotrackInPlaylist(Guid playlistId, Guid audiotrackId)
    {
        return await _context.PlaylistsAudiotracks
            .AnyAsync(pa => pa.AudiotrackId == audiotrackId && pa.PlaylistId == playlistId);
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        var paToDelete = _context.PlaylistsAudiotracks
            .Where(pa => pa.PlaylistId == playlistId);
        _context.PlaylistsAudiotracks.RemoveRange(paToDelete);
        _context.Playlists.Remove(playlistDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Audiotrack>> GetAllAudiotracksFromPlaylist(Guid playlistId)
    {
        var audiofiles = await _context.PlaylistsAudiotracks
            .Where(pa => pa.PlaylistId == playlistId)
            .Include(pa => pa.Audiotrack)
            .Select(pa => AudiotrackConverter.DbToCoreModel(pa.Audiotrack))
            .ToListAsync();
        return audiofiles!;
    }

    public async Task<List<Playlist>> GetAllPlaylists()
    {
        return await _context.Playlists
            .Select(p => PlaylistConverter.DbToCoreModel(p)!)
            .ToListAsync();
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        return PlaylistConverter.DbToCoreModel(playlistDbModel);
    }

    public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
    {
        return await _context.Playlists
            .Where(p => p.UserId == userId)
            .Select(p => PlaylistConverter.DbToCoreModel(p))
            .ToListAsync();
    }

    public async Task RemoveAudiofileFromPlaylist(Guid playlistId, Guid audiofileId)
    {
        var audiofileDbModel = await _context.PlaylistsAudiotracks
            .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId && pa.AudiotrackId == audiofileId);

        if (audiofileDbModel is not null)
        {
            _context.PlaylistsAudiotracks.Remove(audiofileDbModel);
        }
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAudiofilesFromPlaylistBulk(Guid playlistId, List<Guid> audiofileIds)
    {
        foreach (var audiofileId in audiofileIds)
        {
            var audiofileDbModel = await _context.PlaylistsAudiotracks
                .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId && pa.AudiotrackId == audiofileId);

            if (audiofileDbModel is not null)
            {
                _context.PlaylistsAudiotracks.Remove(audiofileDbModel);
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