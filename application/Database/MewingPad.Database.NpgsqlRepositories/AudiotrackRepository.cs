using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class AudiotrackRepository(MewingPadDbContext context) : IAudiotrackRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<AudiotrackRepository>();

    public async Task AddAudiotrack(Audiotrack audiotrack)
    {
        _logger.Verbose("Entering AddAudiotrack method");

        try
        {
            await _context.Audiotracks.AddAsync(AudiotrackConverter.CoreToDbModel(audiotrack));
            await _context.SaveChangesAsync();
            _logger.Information($"Added audiotrack (Id = {audiotrack.Id}) to database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting AddAudiotrack method");
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteAudiotrack method");

        try
        {
            var audiotrackDbModel = await _context.Audiotracks.FindAsync(audiotrackId);
            _context.Audiotracks.Remove(audiotrackDbModel!);
            await _context.SaveChangesAsync();
            _logger.Information($"Deleted audiotrack (Id = {audiotrackId}) from database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteAudiotrack method");
    }

    public async Task<List<Audiotrack>> GetAllAudiotracks()
    {
        _logger.Verbose("Entering GetAllAudiotracks method");

        var audios = await _context.Audiotracks
            .Select(a => AudiotrackConverter.DbToCoreModel(a))
            .ToListAsync();
        if (audios.Count == 0)
        {
            _logger.Warning("Database contains no entries of Audiotrack");
        }

        _logger.Verbose("Exiting GetAllAudiotracks method");
        return audios;
    }

    public async Task<Audiotrack?> GetAudiotrackById(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackById method");

        var audiotrackDbModel = await _context.Audiotracks.FindAsync(audiotrackId);
        if (audiotrackDbModel is null)
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) not found in database");
        }
        var audiotrack = AudiotrackConverter.DbToCoreModel(audiotrackDbModel);

        _logger.Verbose("Exiting GetAudiotrackById method");
        return audiotrack;
    }

    public async Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        _logger.Verbose("Entering GetAudiotracksByTitle method");

        var audiotracks = await _context.Audiotracks
            .Where(a => a.Title == title)
            .Select(a => AudiotrackConverter.DbToCoreModel(a))
            .ToListAsync();
        if (audiotracks.Count == 0)
        {
            _logger.Warning($"Database contains no audiotracks with title \"{title}\"");
        }

        _logger.Verbose("Exiting GetAudiotracksByTitle method");
        return audiotracks!;
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiotrack)
    {
        _logger.Verbose("Entering UpdateAudiotrack method");

        var audiotrackDbModel = await _context.Audiotracks.FindAsync(audiotrack.Id);

        audiotrackDbModel!.Id = audiotrack.Id;
        audiotrackDbModel!.AuthorId = audiotrack.AuthorId;
        audiotrackDbModel!.Title = audiotrack.Title;
        audiotrackDbModel!.Duration = audiotrack.Duration;
        audiotrackDbModel!.Filepath = audiotrack.Filepath;

        await _context.SaveChangesAsync();
        _logger.Information($"Updated audiotrack (Id = {audiotrack.Id})");
        _logger.Verbose("Exiting UpdateAudiotrack method");
        return audiotrack;
    }
}