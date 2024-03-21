using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.ScoreService;

public class ScoreService(IScoreRepository scoreRepository, 
                          IAudiofileRepository audiofileRepository) : IScoreService
{
    private readonly IScoreRepository _scoreRepository = scoreRepository;
    private readonly IAudiofileRepository _audiofileRepository = audiofileRepository;

    public async Task CreateScore(Score score)
    {
        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiofileId) is not null)
        {
            throw new ScoreExistsException(score.AuthorId, score.AudiofileId);
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
        if (await _scoreRepository.GetScoreByPrimaryKey(score.AuthorId, score.AudiofileId) is null)
        {
            throw new ScoreNotFoundException(score.AuthorId, score.AudiofileId);
        }
        return await _scoreRepository.UpdateScore(score);
    }

    public async Task<List<Score>> GetAudiofileScores(Guid audiofileId)
    {
        if (await _audiofileRepository.GetAudiofileById(audiofileId) is null)
        {
            throw new AudiofileNotFoundException(audiofileId);
        }
        return await _scoreRepository.GetAudiofileScores(audiofileId);
    }
}
