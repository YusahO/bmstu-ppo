using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class AudiotrackRepository(MewingPadDbContext context) : IAudiotrackRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddAudiotrack(Audiotrack audiofile)
    {
        await _context.Audiotracks.AddAsync(AudiotrackConverter.CoreToDbModel(audiofile)!);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        var audiofileDbModel = await _context.Audiotracks.FindAsync(audiotrackId);
        var taToDelete = _context.TagsAudiotracks.Where(ta => ta.AudiotrackId == audiotrackId);
        var paToDelete = _context.PlaylistsAudiotracks.Where(pa => pa.AudiotrackId == audiotrackId);
        _context.Audiotracks.Remove(audiofileDbModel!);
        _context.TagsAudiotracks.RemoveRange(taToDelete);
        _context.PlaylistsAudiotracks.RemoveRange(paToDelete);
        await _context.SaveChangesAsync();
    }

    public Task<List<Audiotrack>> GetAllAudiotracks()
    {
        return _context.Audiotracks
            .Select(a => AudiotrackConverter.DbToCoreModel(a))
            .ToListAsync();
    }

    public async Task<Audiotrack?> GetAudiotrackById(Guid audiotrackId)
    {
        var audiofileDbModel = await _context.Audiotracks.FindAsync(audiotrackId);
        return AudiotrackConverter.DbToCoreModel(audiofileDbModel);
    }

    public async Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        var audiotracks = await _context.Audiotracks
            .Where(a => a.Title == title)
            .Select(a => AudiotrackConverter.DbToCoreModel(a))
            .ToListAsync();
        return audiotracks!;
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiofile)
    {
        var audiofileDbModel = await _context.Audiotracks.FindAsync(audiofile.Id);

        audiofileDbModel!.Id = audiofile.Id;
        audiofileDbModel!.AuthorId = audiofile.AuthorId;
        audiofileDbModel!.Title = audiofile.Title;
        audiofileDbModel!.Duration = audiofile.Duration;
        audiofileDbModel!.Filepath = audiofile.Filepath;

        await _context.SaveChangesAsync();
        return AudiotrackConverter.DbToCoreModel(audiofileDbModel)!;
    }
}