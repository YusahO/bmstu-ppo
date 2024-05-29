using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.MongoDB.Models.Converters;
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
            var audios = await _context.Audiotracks
                .Where(a => a.PlaylistIds.Contains(playlistId))
                .ToListAsync();
            for (int i = 0; i < audios.Count; ++i)
            {
                audios[i].PlaylistIds.Remove(playlistId);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteByPlaylist");
    }

    public Task DeleteByAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteByAudiotrack");

        _logger.Information("Nothing to do in MongoDB");

        _logger.Verbose("Exiting DeleteByAudiotrack");
        return Task.CompletedTask;
    }

    public async Task AddAudiotrackToPlaylist(Guid playlistId, Guid audiotrackId)
    {
        _logger.Verbose("Entering AddAudiotrackToPlaylist");

        try
        {
            var audio = await _context.Audiotracks
                .SingleAsync(a => a.Id == audiotrackId);
            audio.PlaylistIds.Add(playlistId);
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
            var audios = await _context.Audiotracks
                .Where(a => a.PlaylistIds.Contains(playlistId))
                .ToListAsync();
            audiotracks = audios.Select(a => AudiotrackConverter.DbToCoreModel(a)).ToList();
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
            var audio = await _context.Audiotracks
                .SingleAsync(a => a.Id == audiotrackId);
            audio.PlaylistIds.Remove(playlistId);
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
                var audio = await _context.Audiotracks
                    .SingleAsync(a => a.Id == aid);
                audio.PlaylistIds.Remove(playlistId);
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
            inPlaylist = await _context.Audiotracks
                .AnyAsync(a => a.Id == audiotrackId && a.PlaylistIds.Contains(playlistId));
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting IsAudiotrackInPlaylist");
        return inPlaylist;
    }
}