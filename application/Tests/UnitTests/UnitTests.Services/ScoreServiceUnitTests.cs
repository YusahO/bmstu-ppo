using MewingPad.Common.Exceptions;
using MewingPad.Services.ScoreService;

namespace UnitTests.Services;

public class ScoreServiceUnitTest
{
    private readonly IScoreService _scoreService;
    private readonly Mock<IScoreRepository> _mockScoreRepository = new();
    private readonly Mock<IAudiotrackRepository> _mockAudiotrackRepository = new();

    public ScoreServiceUnitTest()
    {
        _scoreService = new ScoreService(_mockScoreRepository.Object,
                                         _mockAudiotrackRepository.Object);
    }

    [Fact]
    public async Task TestCreateScore()
    {
        List<Score> scores = [];
        var expectedScore = CreateMockScore(1);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiotrackId))
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

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiotrackId))
                            .ReturnsAsync(expectedScore);

        async Task Action() => await _scoreService.CreateScore(expectedScore);

        await Assert.ThrowsAsync<ScoreExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateScore()
    {
        var expectedScore = CreateMockScore(1);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiotrackId))
                            .ReturnsAsync(expectedScore);

        _mockScoreRepository.Setup(s => s.UpdateScore(It.IsAny<Score>()))
                            .ReturnsAsync(expectedScore);

        var actualScore = await _scoreService.UpdateScore(expectedScore);

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestUpdateScoreNonexistent()
    {
        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(It.IsAny<Guid>(), It.IsAny<Guid>()))
                            .ReturnsAsync(default(Score)!);

        async Task Action() => await _scoreService.UpdateScore(CreateMockScore(3));

        await Assert.ThrowsAsync<ScoreNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetScoreByPrimaryKey()
    {
        var expectedScore = CreateMockScore(3);

        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiotrackId))
                            .ReturnsAsync(expectedScore);

        var actualScore = await _scoreService.GetScoreByPrimaryKey(expectedScore.AuthorId, expectedScore.AudiotrackId);

        Assert.Equal(expectedScore, actualScore);
    }

    [Fact]
    public async Task TestGetScoreNonexistentByPrimaryKey()
    {
        _mockScoreRepository.Setup(s => s.GetScoreByPrimaryKey(It.IsAny<Guid>(), It.IsAny<Guid>()))
                            .ReturnsAsync(default(Score)!);

        async Task Action() => await _scoreService.GetScoreByPrimaryKey(Guid.Empty, Guid.Empty);

        await Assert.ThrowsAsync<ScoreNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetAudiofileScores()
    {
        Audiotrack file = new(Guid.NewGuid(), "title", 4.53f, Guid.NewGuid(), "path/to/file");
        List<Score> expectedScores = [CreateMockScore(1), CreateMockScore(2)];
        foreach (var score in expectedScores)
        {
            score.AudiotrackId = file.Id;
        }

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(file.Id))
                                .ReturnsAsync(file);

        _mockScoreRepository.Setup(s => s.GetAudiotrackScores(file.Id))
                            .ReturnsAsync(expectedScores);

        var actualScores = await _scoreService.GetAudiotrackScores(file.Id);

        Assert.Equal(expectedScores, actualScores);
    }

    [Fact]
    public async Task TestGetAudiofileNonexistentScores()
    {
        var expectedFileId = Guid.NewGuid();
        var fakeFileId = Guid.NewGuid();

        _mockScoreRepository.Setup(s => s.GetAudiotrackScores(fakeFileId))
                            .ReturnsAsync([]);

        async Task Action() => await _scoreService.GetAudiotrackScores(expectedFileId);

        await Assert.ThrowsAsync<AudiotrackNotFoundException>(Action);
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
}