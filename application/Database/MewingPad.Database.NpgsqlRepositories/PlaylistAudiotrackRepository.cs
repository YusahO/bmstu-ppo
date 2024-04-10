using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class PlaylistAudiotrackRepository(MewingPadDbContext context) : IPlaylistAudiotrackRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<PlaylistAudiotrackRepository>();

    public async Task DeleteByPlaylist(Guid playlistId)
    {
        _logger.Information("Entering DeleteByPlaylist method");

        var pairs = await _context.PlaylistsAudiotracks
            .Where(pa => pa.PlaylistId == playlistId)
            .ToListAsync();
        if (pairs.Count == 0)
        {
            _logger.Warning($"Playlist (Id = {playlistId}) not found in database");
            return;
        }

        try
        {
            _context.RemoveRange(pairs);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting DeleteByPlaylist method");
    }

    public async Task DeleteByAudiotrack(Guid audiotrackId)
    {
        _logger.Information("Entering DeleteByAudiotrack method");

        var pairs = await _context.PlaylistsAudiotracks
            .Where(pa => pa.AudiotrackId == audiotrackId)
            .ToListAsync();
        if (pairs.Count == 0)
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) not found in database");
            return;
        }

        try
        {
            _context.RemoveRange(pairs);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting DeleteByAudiotrack method");
    }

    public async Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Information("Entering AddAudiotrackToPlaylist method");

        try
        {
            await _context.PlaylistsAudiotracks
                    .AddAsync(new(playlistId, audiotrackId));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting AddAudiotrackToPlaylist method");
    }

    public async Task<List<Audiotrack>> GetAllAudiotracksFromPlaylist(Guid playlistId)
    {
        _logger.Information("Entering GetAllAudiotracksFromPlaylist method");

        var audiotracks = await _context.PlaylistsAudiotracks
            .Where(pa => pa.PlaylistId == playlistId)
            .Include(pa => pa.Audiotrack)
            .Select(pa => AudiotrackConverter.DbToCoreModel(pa.Audiotrack))
            .ToListAsync();
        if (audiotracks.Count == 0)
        {
            _logger.Warning($"Database contains no audiotracks in playlist with id {playlistId}");
        }

        _logger.Information("Exiting GetAllAudiotracksFromPlaylist method");
        return audiotracks!;
    }

    public async Task RemoveAudiotrackFromPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Information("Entering RemoveAudiotrackFromPlaylist method");

        try
        {
            _context.PlaylistsAudiotracks.Remove(new(playlistId, audiotrackId));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting RemoveAudiotrackFromPlaylist method");
    }

    public async Task RemoveAudiotracksFromPlaylist(Guid playlistId, List<Guid> audiotrackIds)
    {
        _logger.Information("Entering RemoveAudiotracksFromPlaylist method");

        try
        {
            foreach (var aid in audiotrackIds)
            {
                var paDbModel = await _context.PlaylistsAudiotracks
                    .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId &&
                                               pa.AudiotrackId == aid);

                if (paDbModel is not null)
                {
                    _context.PlaylistsAudiotracks.Remove(paDbModel);
                }
                else
                {
                    _logger.Warning($"Audiotrack (Id = {aid}) not found in playlist (Id = {playlistId})");
                }
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting RemoveAudiotracksFromPlaylist method");
    }

    public async Task<bool> IsAudiotrackInPlaylist(Guid audiotrackId, Guid playlistId)
    {
        _logger.Information("Entering IsAudiotrackInPlaylist method");

        var inPlaylist = await _context.PlaylistsAudiotracks
            .AnyAsync(pa => pa.AudiotrackId == audiotrackId &&
                            pa.PlaylistId == playlistId);
        
        _logger.Information("Exiting IsAudiotrackInPlaylist method");
        return inPlaylist;
    }
}