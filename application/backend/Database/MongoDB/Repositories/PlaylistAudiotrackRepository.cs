using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.MongoDB.Repositories;

public class PlaylistAudiotrackRepository(MewingPadMongoDbContext context) : IPlaylistAudiotrackRepository
{
    private readonly MewingPadMongoDbContext _context = context;
    private readonly ILogger _logger = Log.ForContext<AudiotrackRepository>();

    public async Task DeleteByPlaylist(Guid playlistId)
    {
        _logger.Verbose("Entering DeleteByPlaylist");

        try
        {
            var pairs = await _context.Playlists
                .Where(p => p.Id == playlistId)
                .Include(p => p.Audiotracks)
                    .ThenInclude(pa => pa.Id)
                .ToListAsync();
            if (pairs.Count == 0)
            {
                return;
            }
            _context.RemoveRange(pairs);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteByPlaylist");
    }

    public async Task DeleteByAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteByAudiotrack");

        try
        {
            var pairs = await _context.Audiotracks
                .Where(a => a.Id == audiotrackId)
                .Include(a => a.Playlists)
                    .ThenInclude(pa => pa.Id)
                .ToListAsync();
            if (pairs.Count == 0)
            {
                return;
            }
            _context.RemoveRange(pairs);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteByAudiotrack");
    }

    public async Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Verbose("Entering AddAudiotrackToPlaylist");

        try
        {
            var playlist = _context.Playlists
                .Include(p => p.Audiotracks)
                .Single(p => p.Id == playlistId);
            var audiotrack = _context.Audiotracks
                .Single(a => a.Id == audiotrackId);

            playlist.Audiotracks.Add(audiotrack);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddAudiotrackToPlaylist");
    }

    public async Task<List<Audiotrack>> GetAllAudiotracksFromPlaylist(Guid playlistId)
    {
        _logger.Verbose("Entering GetAllAudiotracksFromPlaylist");

        List<Audiotrack> audiotracks;
        try
        {
            // audiotracks = await _context.PlaylistsAudiotracks
            //     .Where(pa => pa.PlaylistId == playlistId)
            //     .Include(pa => pa.Audiotrack)
            //     .Select(pa => AudiotrackConverter.DbToCoreModel(pa.Audiotrack))
            //     .ToListAsync();
            audiotracks = await _context.Playlists
                .Where(p => p.Id == playlistId)
                .SelectMany(p => p.Audiotracks)
                .Select(a => AudiotrackConverter.DbToCoreModel(a))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllAudiotracksFromPlaylist");
        return audiotracks!;
    }

    public async Task RemoveAudiotrackFromPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Verbose("Entering RemoveAudiotrackFromPlaylist");

        try
        {
            var playlist = _context.Playlists
                .Include(p => p.Audiotracks)
                .Single(p => p.Id == playlistId);
            var audiotrack = _context.Audiotracks
                .Single(a => a.Id == audiotrackId);

            playlist.Audiotracks.Remove(audiotrack);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting RemoveAudiotrackFromPlaylist");
    }

    public async Task RemoveAudiotracksFromPlaylist(Guid playlistId, List<Guid> audiotrackIds)
    {
        _logger.Verbose("Entering RemoveAudiotracksFromPlaylist");

        try
        {
            foreach (var aid in audiotrackIds)
            {
                var playlist = _context.Playlists
                    .Include(p => p.Audiotracks)
                    .Single(p => p.Id == playlistId);
                var audiotrack = _context.Audiotracks
                    .Single(a => a.Id == aid);
                
                playlist.Audiotracks.Remove(audiotrack);
                // var paDbModel = await _context.PlaylistsAudiotracks
                //     .FirstOrDefaultAsync(pa => pa.PlaylistId == playlistId &&
                //                                pa.AudiotrackId == aid);

                // if (paDbModel is not null)
                // {
                //     _context.PlaylistsAudiotracks.Remove(paDbModel);
                // }
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting RemoveAudiotracksFromPlaylist");
    }

    public async Task<bool> IsAudiotrackInPlaylist(Guid audiotrackId, Guid playlistId)
    {
        _logger.Verbose("Entering IsAudiotrackInPlaylist");

        bool inPlaylist;
        try
        {
            inPlaylist = await _context.Playlists
                .AnyAsync(p => p.Id == playlistId 
                               && p.Audiotracks.Any(pa => pa.Id == audiotrackId));
            // inPlaylist = await _context.PlaylistsAudiotracks
            //        .AnyAsync(pa => pa.AudiotrackId == audiotrackId &&
            //                        pa.PlaylistId == playlistId);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting IsAudiotrackInPlaylist");
        return inPlaylist;
    }
}