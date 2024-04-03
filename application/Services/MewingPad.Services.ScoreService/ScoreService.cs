using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.ScoreService;

public class ScoreService(IScoreRepository scoreRepository, 
                          IAudiotrackRepository audiofileRepository) : IScoreService
{
    private readonly IScoreRepository _scoreRepository = scoreRepository;
    private readonly IAudiotrackRepository _audiofileRepository = audiofileRepository;

    public async Task CreateScore(Score score)
    {
        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiotrackId) is not null)
        {
            throw new ScoreExistsException(score.AuthorId, score.AudiotrackId);
        }
        await _scoreRepository.AddScore(score);
    }

    public async Task<Score> GetScoreByPrimaryKey(Guid authorId, Guid audiofileId)
    {
        var score = await _scoreRepository.GetScoreByPrimaryKey(authorId, audiofileId)
                    ?? throw new ScoreNotFoundException(authorId, audiofileId);
        return score;
    }

    public async Task<Score> UpdateScore(Score score)
    {
        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiotrackId) is null)
        {
            throw new ScoreNotFoundException(score.AuthorId, score.AudiotrackId);
        }
        return await _scoreRepository.UpdateScore(score);
    }

    public async Task<List<Score>> GetAudiotrackScores(Guid audiofileId)
    {
        if (await _audiofileRepository.GetAudiotrackById(audiofileId) is null)
        {
            throw new AudiotrackNotFoundException(audiofileId);
        }
        return await _scoreRepository.GetAudiotrackScores(audiofileId);
    }
}
