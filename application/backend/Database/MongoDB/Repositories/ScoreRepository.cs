using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.MongoDB.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.MongoDB.Repositories;

public class ScoreRepository(MewingPadMongoDbContext context) : IScoreRepository
{
	private readonly MewingPadMongoDbContext _context = context;
	private readonly ILogger _logger = Log.ForContext<AudiotrackRepository>();

    public async Task AddScore(Score score)
    {
        _logger.Verbose("Entering AddScore");

        try
        {
            await _context.Scores.AddAsync(ScoreConverter.CoreToDbModel(score));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddScore");
    }

    public async Task DeleteScore(Guid authorId, Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteScore");

        try
        {
            var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
            _context.Scores.Remove(scoreDbModel!);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteScore");
    }

    public async Task<List<Score>> GetAllScores()
    {
        _logger.Verbose("Entering GetAllScores");

        List<Score> scores;
        try
        {
            var found = await _context.Scores.ToListAsync();
            scores = found.Select(s => ScoreConverter.DbToCoreModel(s)).ToList();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllScores");
        return scores;
    }

    public async Task<List<Score>> GetAudiotrackScores(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackScores");

        List<Score> scores;
        try
        {
            var found = await _context.Scores
                    .Where(s => s.AudiotrackId == audiotrackId)
                    .ToListAsync();
            scores = found.Select(s => ScoreConverter.DbToCoreModel(s)).ToList();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAudiotrackScores");
        return scores!;
    }

    public async Task<Score?> GetScoreByPrimaryKey(Guid authorId, Guid audiotrackId)
    {
        _logger.Verbose("Entering GetScoreByPrimaryKey");

        Score? score;
        try
        {
            var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
            score = ScoreConverter.DbToCoreModel(scoreDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetScoreByPrimaryKey");
        return score;
    }

    public async Task<Score> UpdateScore(Score score)
    {
        _logger.Verbose("Entering UpdateScore");

        try
        {
            var scoreDbModel = await _context.Scores.FindAsync([score.AuthorId, score.AudiotrackId]);

            scoreDbModel!.AuthorId = score.AuthorId;
            scoreDbModel!.AudiotrackId = score.AudiotrackId;
            scoreDbModel!.Value = score.Value;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting UpdateScore");
        return score;
    }
}