using MewingPad.Common.Exceptions;
using MewingPad.Services.AudiotrackService;

namespace UnitTests.Services;

public class AudiofileServiceUnitTest
{
    private readonly IAudiotrackService _audiotrackService;
    private readonly Mock<IAudiotrackRepository> _mockAudiotrackRepository = new();

    public AudiofileServiceUnitTest()
    {
        _audiotrackService = new AudiotrackService(_mockAudiotrackRepository.Object);
    }

    [Fact]
    public async Task TestCreateAudiofile()
    {
        List<Audiotrack> files = [];
        var expectedFile = CreateMockAudiofile();

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(expectedFile.Id))
                                .ReturnsAsync(default(Audiotrack)!);

        _mockAudiotrackRepository.Setup(s => s.AddAudiotrack(It.IsAny<Audiotrack>()))
                                .Callback((Audiotrack f) => files.Add(f));

        await _audiotrackService.CreateAudiotrack(expectedFile);
        var actualFile = files.Last();

        Assert.Equal(expectedFile, actualFile);
    }

    [Fact]
    public async Task TestCreateExistentAudiofile()
    {
        var expectedFile = CreateMockAudiofile();

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        async Task Action() => await _audiotrackService.CreateAudiotrack(expectedFile);

        await Assert.ThrowsAsync<AudiotrackExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateAudiofile()
    {
        var expectedFile = CreateMockAudiofile();

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        _mockAudiotrackRepository.Setup(s => s.UpdateAudiotrack(It.IsAny<Audiotrack>()))
                                .ReturnsAsync(expectedFile);

        var actualFile = await _audiotrackService.UpdateAudiotrack(expectedFile);

        Assert.Equal(actualFile.Duration, expectedFile.Duration);
    }

    [Fact]
    public async Task TestUpdateAudiofileNonexistent()
    {
        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiotrack)!);

        async Task Action() => await _audiotrackService.UpdateAudiotrack(CreateMockAudiofile());

        await Assert.ThrowsAsync<AudiotrackNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        List<Audiotrack> files = [CreateMockAudiofile()];
        var expectedFile = new Audiotrack(files.First());

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        _mockAudiotrackRepository.Setup(s => s.DeleteAudiotrack(It.IsAny<Guid>()))
                                .Callback((Guid id) =>
                                {
                                    files.Remove(expectedFile);
                                });

        await _audiotrackService.DeleteAudiotrack(expectedFile.Id);

        Assert.Empty(files);
    }

    [Fact]
    public async Task TestDeleteAudiofileNonexistent()
    {
        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiotrack)!);

        async Task Action() => await _audiotrackService.DeleteAudiotrack(Guid.Empty);

        await Assert.ThrowsAsync<AudiotrackNotFoundException>(Action);
    }

    private static Audiotrack CreateMockAudiofile(string title = "title",
                                                 float duration = 1.1f,
                                                 string filepath = "path/to/mock")
    {
        return new(Guid.NewGuid(), title, duration, Guid.NewGuid(), filepath);
    }
}