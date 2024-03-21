using MewingPad.Common.Exceptions;
using MewingPad.Services.ScoreService;

namespace UnitTests.Services;

public class ScoreServiceUnitTest
{
    private readonly IScoreService _scoreService;
    private readonly Mock<IScoreRepository> _mockScoreRepository = new();
    private readonly Mock<IAudiofileRepository> _mockAudiofileRepository = new();

    public ScoreServiceUnitTest()
    {
        _scoreService = new ScoreService(_mockScoreRepository.Object,
                                         _mockAudiofileRepository.Object);
    }

    [Fact]
    public async Task TestCreateScore()
    {
        var scores = CreateMockScores();
        var expectedScore = CreateMockScore(1);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiofileId))
                            .ReturnsAsync(default(Score)!);

        _mockScoreRepository.Setup(s => s.AddScore(It.IsAny<Score>()))
                            .Callback((Score sc) => scores.Add(sc));

        await _scoreService.CreateScore(expectedScore);
        var actualScore = scores.Last();

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestCreateExistentScore()
    {
        var expectedScore = CreateMockScore(1);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiofileId))
                            .ReturnsAsync(expectedScore);

        async Task Action() => await _scoreService.CreateScore(expectedScore);

        await Assert.ThrowsAsync<ScoreExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateScore()
    {
        var actualScore = CreateMockScore(1);
        var expectedScore = actualScore;
        expectedScore.SetValue(3);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(actualScore.AuthorId, actualScore.AudiofileId))
                            .ReturnsAsync(actualScore);

        _mockScoreRepository.Setup(s => s.UpdateScore(It.IsAny<Score>()))
                            .Callback((Score sc) =>
                            {
                                actualScore = expectedScore;
                            });

        await _scoreService.UpdateScore(expectedScore);

        Assert.Equal(actualScore, expectedScore);
    }

    [Fact]
    public async Task TestUpdateScoreNonexistent()
    {
        var expectedScore = CreateMockScore(4);
        var actualScore = CreateMockScore(5);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(actualScore.AuthorId, actualScore.AudiofileId))
                            .ReturnsAsync(actualScore);

        async Task Action() => await _scoreService.UpdateScore(expectedScore);

        await Assert.ThrowsAsync<ScoreNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetScoreByPrimaryKey()
    {
        var expectedScore = CreateMockScore(3);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiofileId))
                            .ReturnsAsync(expectedScore);

        var score = await _scoreService.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiofileId);

        Assert.Equal(expectedScore, score);
    }

    [Fact]
    public async Task TestGetScoreNonexistentByPrimaryKey()
    {
        var expectedScore = CreateMockScore(4);
        var actualScore = CreateMockScore(1);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(actualScore.AuthorId, actualScore.AudiofileId))
                            .ReturnsAsync(actualScore);

        async Task Action() => await _scoreService.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiofileId);

        await Assert.ThrowsAsync<ScoreNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetAudiofileScores()
    {
        Audiofile file = new(Guid.NewGuid(), "title", 4.53f, Guid.NewGuid(), "path/to/file");
        var expectedScores = CreateMockScores();
        foreach (var score in expectedScores)
        {
            score.AudiofileId = file.Id;
        }

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(file.Id))
                                .ReturnsAsync(file);

        _mockScoreRepository.Setup(s => s.GetAudiofileScores(file.Id))
                            .ReturnsAsync(expectedScores);

        var actualScores = await _scoreService.GetAudiofileScores(file.Id);

        Assert.Equal(expectedScores, actualScores);
    }

    [Fact]
    public async Task TestGetAudiofileNonexistentScores()
    {
        var expectedFileId = Guid.NewGuid();
        var fakeFileId = Guid.NewGuid();

        _mockScoreRepository.Setup(s => s.GetAudiofileScores(fakeFileId))
                            .ReturnsAsync([]);

        async Task Action() => await _scoreService.GetAudiofileScores(expectedFileId);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public void TestScoreConstructInvalidValue()
    {
        void Action() => new Score(Guid.Empty, Guid.Empty, 10);
        Assert.Throws<ScoreInvalidValueException>(Action);
    }

    [Fact]
    public void TestScoreSetInvalidValue()
    {
        var score = new Score(Guid.Empty, Guid.Empty, 3);
        void Action() => score.SetValue(-1);
        Assert.Throws<ScoreInvalidValueException>(Action);
    }

    private static Score CreateMockScore(int value)
    {
        return new(Guid.NewGuid(), Guid.NewGuid(), value);
    }

    private static List<Score> CreateMockScores()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), 4),
            new(Guid.NewGuid(), Guid.NewGuid(), 1),
            new(Guid.NewGuid(), Guid.NewGuid(), 2),
            new(Guid.NewGuid(), Guid.NewGuid(), 5)
        ];
    }
}