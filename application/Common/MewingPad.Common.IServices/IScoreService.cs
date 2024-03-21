using MewingPad.Common.Entities;

namespace MewingPad.Services.ScoreService;

public interface IScoreService
{
    Task CreateScore(Score score);
    Task<Score> UpdateScore(Score score);
    Task<Score> GetScoreByPrimaryKey(Guid authorId, Guid audiofileId);
    Task<List<Score>> GetAudiofileScores(Guid audiofileId);
}