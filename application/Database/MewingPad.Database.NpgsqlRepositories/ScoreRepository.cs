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
        await _context.Scores.AddAsync(ScoreConverter.CoreToDbModel(score));
        await _context.SaveChangesAsync();
    }

    public async Task DeleteScore(Guid authorId, Guid audiotrackId)
    {
        var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
        _context.Scores.Remove(scoreDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Score>> GetAllScores()
    {
        return await _context.Scores
            .Select(s => ScoreConverter.DbToCoreModel(s))
            .ToListAsync();
    }

    public async Task<List<Score>> GetAudiotrackScores(Guid audiotrackId)
    {
        var scores = await _context.Scores
            .Where(s => s.AudiotrackId == audiotrackId)
            .Select(s => ScoreConverter.DbToCoreModel(s))
            .ToListAsync();
        return scores!;
    }

    public async Task<Score?> GetScoreByPrimaryKey(Guid authorId, Guid audiotrackId)
    {
        var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
        return ScoreConverter.DbToCoreModel(scoreDbModel);
    }

    public async Task<Score> UpdateScore(Score score)
    {
        var scoreDbModel = await _context.Scores.FindAsync([score.AuthorId, score.AudiotrackId]);

        scoreDbModel!.AuthorId = score.AuthorId;
        scoreDbModel!.AudiotrackId = score.AudiotrackId;
        scoreDbModel!.Value = score.Value;

        await _context.SaveChangesAsync();
        return ScoreConverter.DbToCoreModel(scoreDbModel);
    }
}