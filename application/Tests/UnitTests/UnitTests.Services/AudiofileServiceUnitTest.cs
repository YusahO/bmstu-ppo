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
        List<Audiofile> files = [];
        var expectedFile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFile.Id))
                                .ReturnsAsync(default(Audiofile)!);

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
        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiofile)!);

        async Task Action() => await _audiofileService.UpdateAudiofile(CreateMockAudiofile());

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        List<Audiofile> files = [CreateMockAudiofile()];
        var expectedFile = new Audiofile(files.First());

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(expectedFile.Id))
                                .ReturnsAsync(expectedFile);

        _mockAudiofileRepository.Setup(s => s.DeleteAudiofile(It.IsAny<Guid>()))
                                .Callback((Guid id) =>
                                {
                                    files.Remove(expectedFile);
                                });

        await _audiofileService.DeleteAudiofile(expectedFile.Id);

        Assert.Empty(files);
    }

    [Fact]
    public async Task TestDeleteAudiofileNonexistent()
    {
        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiofile)!);

        async Task Action() => await _audiofileService.DeleteAudiofile(Guid.Empty);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    private static Audiofile CreateMockAudiofile(string title = "title",
                                                 float duration = 1.1f,
                                                 string filepath = "path/to/mock")
    {
        return new(Guid.NewGuid(), title, duration, Guid.NewGuid(), filepath);
    }
}