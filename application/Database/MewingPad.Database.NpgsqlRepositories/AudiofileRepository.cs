using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Utils.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class AudiofileRepository(MewingPadDbContext context) : IAudiofileRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddAudiofile(Audiofile audiofile)
    {
        await _context.Audiofiles.AddAsync(AudiofileConverter.CoreToDbModel(audiofile));
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAudiofile(Guid audiofileId)
    {
        var audiofileDbModel = await _context.Audiofiles.FindAsync(audiofileId);
        _context.Remove(audiofileDbModel!);
        await _context.SaveChangesAsync();
    }

    public Task<List<Audiofile>> GetAllAudiofiles()
    {
        return _context.Audiofiles
            .Select(a => AudiofileConverter.DbToCoreModel(a))
            .ToListAsync();
    }

    public async Task<Audiofile> GetAudiofileById(Guid audiofileId)
    {
        var audiofileDbModel = await _context.Audiofiles.FindAsync(audiofileId);
        return AudiofileConverter.DbToCoreModel(audiofileDbModel);
    }

    public async Task<Audiofile> UpdateAudiofile(Audiofile audiofile)
    {
        var audiofileDbModel = await _context.Audiofiles.FindAsync(audiofile.Id);

        audiofileDbModel!.Id = audiofile.Id;
        audiofileDbModel!.AuthorId = audiofile.AuthorId;
        audiofileDbModel!.Title = audiofile.Title;
        audiofileDbModel!.Duration = audiofile.Duration;
        audiofileDbModel!.Filepath = audiofile.Filepath;

        await _context.SaveChangesAsync();
        return AudiofileConverter.DbToCoreModel(audiofileDbModel);
    }
}