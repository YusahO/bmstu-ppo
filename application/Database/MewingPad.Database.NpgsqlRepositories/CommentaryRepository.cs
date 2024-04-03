using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.NpgsqlRepositories;

public class CommentaryRepository(MewingPadDbContext context) : ICommentaryRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddCommentary(Commentary commentary)
    {
        await _context.Commentaries.AddAsync(CommentaryConverter.CoreToDbModel(commentary)!);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentary(Guid commentaryId)
    {
        var commentaryDbModel = await _context.Commentaries.FindAsync(commentaryId);
        _context.Commentaries.Remove(commentaryDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Commentary>> GetAudiotrackCommentaries(Guid audiofileId)
    {
        return await _context.Commentaries
            .Where(c => c.AudiotrackId == audiofileId)
            .Select(c => CommentaryConverter.DbToCoreModel(c))
            .ToListAsync();
    }

    public async Task<Commentary?> GetCommentaryById(Guid commentaryId)
    {
        var commentaryDbModel = await _context.Commentaries.FindAsync(commentaryId);
        return CommentaryConverter.DbToCoreModel(commentaryDbModel);
    }

    public async Task<Commentary> UpdateCommentary(Commentary commentary)
    {
        var commentaryDbModel = await _context.Commentaries.FindAsync(commentary.Id);

        commentaryDbModel!.Id = commentary.Id;
        commentaryDbModel!.AuthorId = commentary.AuthorId;
        commentaryDbModel!.AudiotrackId = commentary.AudiotrackId;
        commentaryDbModel!.Text = commentary.Text;

        await _context.SaveChangesAsync();
        return CommentaryConverter.DbToCoreModel(commentaryDbModel);
    }
}