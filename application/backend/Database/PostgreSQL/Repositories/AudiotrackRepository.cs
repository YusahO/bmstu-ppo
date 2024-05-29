using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.PgSQL.Context;
using MewingPad.Database.PgSQL.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.PgSQL.Repositories;

public class AudiotrackRepository(MewingPadPgSQLDbContext context) : IAudiotrackRepository
{
    private readonly MewingPadPgSQLDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<AudiotrackRepository>();

    public async Task AddAudiotrack(Audiotrack audiotrack)
    {
        _logger.Verbose("Entering AddAudiotrack");

        try
        {
            await _context.Audiotracks.AddAsync(AudiotrackConverter.CoreToDbModel(audiotrack));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddAudiotrack");
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteAudiotrack");

        try
        {
            await _context.Audiotracks.Where(a => a.Id == audiotrackId).ExecuteDeleteAsync();
            // await _context.PlaylistsAudiotracks.Where(pa => pa.AudiotrackId == audiotrackId).ExecuteDeleteAsync();
            // await _context.TagsAudiotracks.Where(ta => ta.AudiotrackId == audiotrackId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteAudiotrack");
    }

    public async Task<List<Audiotrack>> GetAllAudiotracks()
    {
        _logger.Verbose("Entering GetAllAudiotracks");

        List<Audiotrack> audios;
        try
        {
            audios = await _context.Audiotracks
                    .Select(a => AudiotrackConverter.DbToCoreModel(a))
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllAudiotracks");
        return audios;
    }

    public async Task<Audiotrack?> GetAudiotrackById(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackById");

        Audiotrack? audiotrack;
        try
        {
            var audiotrackDbModel = await _context.Audiotracks.FindAsync(audiotrackId);
            audiotrack = AudiotrackConverter.DbToCoreModel(audiotrackDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAudiotrackById");
        return audiotrack;
    }

    public async Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        _logger.Verbose("Entering GetAudiotracksByTitle");

        List<Audiotrack> audiotracks;
        try
        {
            audiotracks = await _context.Audiotracks
                    .Where(a => a.Title.Contains(title))
                    .Select(a => AudiotrackConverter.DbToCoreModel(a))
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAudiotracksByTitle");
        return audiotracks!;
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiotrack)
    {
        _logger.Verbose("Entering UpdateAudiotrack");

        try
        {
            var audiotrackDbModel = await _context.Audiotracks.FindAsync(audiotrack.Id);

            audiotrackDbModel!.Id = audiotrack.Id;
            audiotrackDbModel!.AuthorId = audiotrack.AuthorId;
            audiotrackDbModel!.Title = audiotrack.Title;
            audiotrackDbModel!.Duration = audiotrack.Duration;
            audiotrackDbModel!.Filepath = audiotrack.Filepath;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting UpdateAudiotrack");
        return audiotrack;
    }
}