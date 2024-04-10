using Microsoft.EntityFrameworkCore;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class ScoreRepository(MewingPadDbContext context) : IScoreRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<ScoreRepository>();

    public async Task AddScore(Score score)
    {
        _logger.Verbose("Entering AddScore method");

        try
        {
            await _context.Scores.AddAsync(ScoreConverter.CoreToDbModel(score));
            await _context.SaveChangesAsync();
            _logger.Information("Score ({@Score}) added to database", score);
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting AddScore method");
    }

    public async Task DeleteScore(Guid authorId, Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteScore method");

        try
        {
            var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
            _context.Scores.Remove(scoreDbModel!);
            await _context.SaveChangesAsync();
            _logger.Information($"Deleted score (AuthorId = {authorId}, AudiortrackId = {audiotrackId})");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteScore method");
    }

    public async Task<List<Score>> GetAllScores()
    {
        _logger.Verbose("Entering GetAllScores method");

        var scores = await _context.Scores
            .Select(s => ScoreConverter.DbToCoreModel(s))
            .ToListAsync();
        if (scores.Count == 0)
        {
            _logger.Warning("Database has no entries of Score");
        }

        _logger.Verbose("Exiting GetAllScores method");
        return scores;
    }

    public async Task<List<Score>> GetAudiotrackScores(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackScores method");

        var scores = await _context.Scores
            .Where(s => s.AudiotrackId == audiotrackId)
            .Select(s => ScoreConverter.DbToCoreModel(s))
            .ToListAsync();
        if (scores.Count == 0)
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) has no scores");
        }

        _logger.Verbose("Exiting GetAudiotrackScores method");
        return scores!;
    }

    public async Task<Score?> GetScoreByPrimaryKey(Guid authorId, Guid audiotrackId)
    {
        _logger.Verbose("Entering GetScoreByPrimaryKey method");

        var scoreDbModel = await _context.Scores.FindAsync([authorId, audiotrackId]);
        if (scoreDbModel is null)
        {
            _logger.Warning($"Score (AuthorId = {authorId}, AudiortrackId = {audiotrackId}) not found in database");
        }
        var score = ScoreConverter.DbToCoreModel(scoreDbModel);

        _logger.Verbose("Exiting GetScoreByPrimaryKey method");
        return score;
    }

    public async Task<Score> UpdateScore(Score score)
    {
        _logger.Verbose("Entering UpdateScore method");

        var scoreDbModel = await _context.Scores.FindAsync([score.AuthorId, score.AudiotrackId]);

        scoreDbModel!.AuthorId = score.AuthorId;
        scoreDbModel!.AudiotrackId = score.AudiotrackId;
        scoreDbModel!.Value = score.Value;

        await _context.SaveChangesAsync();
        _logger.Information($"Score (AuthorId = {score.AuthorId}, AudiortrackId = {score.AudiotrackId}) updated");
        _logger.Verbose("Exiting UpdateScore method");
        return score;
    }
}