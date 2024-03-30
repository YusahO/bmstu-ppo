using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class ScoreRepository(MewingPadDbContext context) : IScoreRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddScore(Score score)
    {
        await _context.Scores.AddAsync(ScoreConverter.CoreToDbModel(score)!);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteScore(Guid authorId, Guid audiofileId)
    {
        var scoreDbModel = await _context.Scores.FindAsync([authorId, audiofileId]);
        _context.Scores.Remove(scoreDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Score>> GetAllScores()
    {
        return await _context.Scores
            .Select(s => ScoreConverter.DbToCoreModel(s)!)
            .ToListAsync();
    }

    public async Task<List<Score>> GetAudiofileScores(Guid audiofileId)
    {
        var scores = await _context.Scores
            .Where(s => s.AudiofileId == audiofileId)
            .Select(s => ScoreConverter.DbToCoreModel(s))
            .ToListAsync();
        return scores!;
    }

    public async Task<Score?> GetScoreByPrimaryKey(Guid authorId, Guid audiofileId)
    {
        var scoreDbModel = await _context.Scores.FindAsync([authorId, audiofileId]);
        return ScoreConverter.DbToCoreModel(scoreDbModel);
    }

    public async Task<Score> UpdateScore(Score score)
    {
        var scoreDbModel = await _context.Scores.FindAsync([score.AuthorId, score.AudiofileId]);

        scoreDbModel!.AuthorId = score.AuthorId;
        scoreDbModel!.AudiofileId = score.AudiofileId;
        scoreDbModel!.Value = score.Value;

        await _context.SaveChangesAsync();
        return ScoreConverter.DbToCoreModel(scoreDbModel)!;
    }
}