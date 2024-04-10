using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using Serilog;

namespace MewingPad.Services.ScoreService;

public class ScoreService(IScoreRepository scoreRepository,
                          IAudiotrackRepository audiotrackRepository) : IScoreService
{
    private readonly IScoreRepository _scoreRepository = scoreRepository;
    private readonly IAudiotrackRepository _audiotrackRepository = audiotrackRepository;

    private readonly ILogger _logger = Log.ForContext<ScoreService>();

    public async Task CreateScore(Score score)
    {
        _logger.Verbose("Entering CreateScore method");

        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiotrackId) is not null)
        {
            _logger.Error($"Score (AuthorId = {score.AuthorId}, AudiotrackId = {score.AudiotrackId}) already exists");
            throw new ScoreExistsException(score.AuthorId, score.AudiotrackId);
        }
        await _scoreRepository.AddScore(score);

        _logger.Verbose("Exiting CreateScore method");
    }

    public async Task<Score> GetScoreByPrimaryKey(Guid authorId, Guid audiotrackId)
    {
        _logger.Verbose("Entering GetScoreByPrimaryKey method");

        var score = await _scoreRepository.GetScoreByPrimaryKey(authorId, audiotrackId);
        if (score is null)
        {
            _logger.Error($"Score (AuthorId = {authorId}, AudiotrackId = {audiotrackId}) not found");
            throw new ScoreNotFoundException(authorId, audiotrackId);
        }

        _logger.Verbose("Exiting GetScoreByPrimaryKey method");
        return score;
    }

    public async Task<Score> UpdateScore(Score score)
    {
        _logger.Verbose("Entering UpdateScore method");

        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiotrackId) is null)
        {
            _logger.Error($"Score (AuthorId = {score.AuthorId}, AudiotrackId = {score.AudiotrackId}) not found");
            throw new ScoreNotFoundException(score.AuthorId, score.AudiotrackId);
        }
        await _scoreRepository.UpdateScore(score);

        _logger.Verbose("Exiting UpdateScore method");
        return score;
    }

    public async Task<List<Score>> GetAudiotrackScores(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackScores method");

        if (await _audiotrackRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        var scores = await _scoreRepository.GetAudiotrackScores(audiotrackId);

         _logger.Verbose("Exiting GetAudiotrackScores method");
        return scores;
    }
}
