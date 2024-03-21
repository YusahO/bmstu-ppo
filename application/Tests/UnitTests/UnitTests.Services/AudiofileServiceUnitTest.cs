using MewingPad.Common.Exceptions;
using MewingPad.Services.AudiofileService;

namespace UnitTests.Services;

public class AudiofileServiceUnitTest
{
    private readonly IAudiofileService _audiofileService;
    private readonly Mock<IAudiofileRepository> _mockAudiofileRepository = new();

    public AudiofileServiceUnitTest()
    {
        _audiofileService = new AudiofileService(_mockAudiofileRepository.Object);
    }

    [Fact]
    public async Task TestCreateAudiofile()
    {
        var files = CreateMockAudiofiles();
        var expectedFile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFile.Id))
                                .ReturnsAsync(files.Find(f => f.Id == expectedFile.Id)!);

        _mockAudiofileRepository.Setup(s => s.AddAudiofile(It.IsAny<Audiofile>()))
                                .Callback((Audiofile f) => files.Add(f));

        await _audiofileService.CreateAudiofile(expectedFile);
        var actualFile = files.Last();

        Assert.Equal(expectedFile, actualFile);
    }

    [Fact]
    public async Task TestCreateExistentAudiofile()
    {
        var expectedFile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        async Task Action() => await _audiofileService.CreateAudiofile(expectedFile);

        await Assert.ThrowsAsync<AudiofileExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateAudiofile()
    {
        var expectedFile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        _mockAudiofileRepository.Setup(s => s.UpdateAudiofile(It.IsAny<Audiofile>()))
                                .ReturnsAsync(expectedFile);

        var actualFile = await _audiofileService.UpdateAudiofile(expectedFile);

        Assert.Equal(actualFile.Duration, expectedFile.Duration);
    }

    [Fact]
    public async Task TestUpdateAudiofileNonexistent()
    {
        var actualFile = CreateMockAudiofile(duration:14.0f);
        var expectedFile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(actualFile.Id))
                                .ReturnsAsync(actualFile);

        async Task Action() => await _audiofileService.UpdateAudiofile(expectedFile);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        var files = CreateMockAudiofiles();
        var expectedFileId = files[1].Id;

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFileId))
                                .ReturnsAsync(files.Find(f => f.Id == expectedFileId)!);

        _mockAudiofileRepository.Setup(s => s.DeleteAudiofile(It.IsAny<Guid>()))
                                .Callback((Guid id) =>
                                {
                                    files.Remove(files.Find(f => f.Id == id)!);
                                });

        await _audiofileService.DeleteAudiofile(expectedFileId);

        Assert.Equal(2, files.Count);
    }

    [Fact]
    public async Task TestDeleteAudiofileNonexistent()
    {
        var files = CreateMockAudiofiles();
        var expectedFileId = Guid.NewGuid();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFileId))
                                .ReturnsAsync(files.Find(f => f.Id == expectedFileId)!);

        async Task Action() => await _audiofileService.DeleteAudiofile(expectedFileId);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    private static Audiofile CreateMockAudiofile(string title = "title",
                                                 float duration = 1.1f,
                                                 string filepath = "path/to/mock")
    {
        return new(Guid.NewGuid(), title, duration, Guid.NewGuid(), filepath);
    }

    private static List<Audiofile> CreateMockAudiofiles()
    {
        return
        [
            new(Guid.NewGuid(), "title1", 5.43f, Guid.NewGuid(), "path/to/file1"),
            new(Guid.NewGuid(), "title2", 2.00f, Guid.NewGuid(), "path/to/file2"),
            new(Guid.NewGuid(), "title3", 10.03f, Guid.NewGuid(), "path/to/file3"),
        ];
    }

    private static List<Score> CreateMockAudiofileScores(List<Audiofile> files)
    {
        return
        [
            new(Guid.NewGuid(), files[1].Id, 3),
            new(Guid.NewGuid(), files[0].Id, 1),
            new(Guid.NewGuid(), files[2].Id, 5),
            new(Guid.NewGuid(), files[2].Id, 4),
        ];
    }
}