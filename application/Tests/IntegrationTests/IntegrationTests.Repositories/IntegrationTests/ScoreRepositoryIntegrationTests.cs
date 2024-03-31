using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class ScoreRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly IScoreRepository _scoreRepository;

    public ScoreRepositoryIntegrationTests()
    {
        _scoreRepository = new ScoreRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateScore()
    {
        var expectedScore = new Score(Guid.NewGuid(), Guid.NewGuid(), 4);
        
        await _scoreRepository.AddScore(expectedScore);
        var actualScore = await _dbFixture.GetScoreByPrimaryKey(expectedScore.AuthorId,
                                                                expectedScore.AudiofileId);

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestGetScoreByPrimaryKey()
    {
        var scores = InMemoryDbFixture.CreateMockScores();
        await _dbFixture.InsertScores(scores);

        var expectedScore = new Score(scores.First());

        var actualScore = await _scoreRepository.GetScoreByPrimaryKey(expectedScore.AuthorId,
                                                                      expectedScore.AudiofileId);

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestUpdateScore()
    {
        var scores = InMemoryDbFixture.CreateMockScores();
        await _dbFixture.InsertScores(scores);

        var expectedScore = new Score(scores.First());
        expectedScore.SetValue(5);

        var actualScore = await _scoreRepository.UpdateScore(expectedScore);

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestGetAudiofileScores()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        var expectedAudiofile = audiofiles.First();

        var expectedScores = InMemoryDbFixture.CreateMockScores();
        foreach (var score in expectedScores)
        {
            score.AudiofileId = expectedAudiofile.Id;
        }

        await _dbFixture.InsertScores(expectedScores);
        await _dbFixture.InsertAudiofiles(audiofiles);

        var actualScores = await _scoreRepository.GetAudiofileScores(expectedAudiofile.Id);

        Assert.Equal(expectedScores, actualScores);
    }

    [Fact]
    public async Task TestDeleteScore()
    {
        var scores = InMemoryDbFixture.CreateMockScores();
        await _dbFixture.InsertScores(scores);

        var expectedScore = scores.First();
        await _scoreRepository.DeleteScore(expectedScore.AuthorId, expectedScore.AudiofileId);

        var actualScore = await _dbFixture.GetScoreByPrimaryKey(expectedScore.AuthorId,
                                                                expectedScore.AudiofileId);

        Assert.Null(actualScore);
    }

    public void Dispose() => _dbFixture.Dispose();
}