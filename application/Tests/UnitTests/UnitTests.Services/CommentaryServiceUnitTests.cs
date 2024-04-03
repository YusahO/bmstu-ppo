using MewingPad.Common.Exceptions;
using MewingPad.Services.CommentaryService;

namespace UnitTests.Services;

public class CommentaryServiceUnitTest
{
    private readonly ICommentaryService _commentaryService;
    private readonly Mock<ICommentaryRepository> _mockCommentaryRepository = new();
    private readonly Mock<IAudiotrackRepository> _mockAudiotrackRepository = new();

    public CommentaryServiceUnitTest()
    {
        _commentaryService = new CommentaryService(_mockCommentaryRepository.Object,
                                                   _mockAudiotrackRepository.Object);
    }

    [Fact]
    public async Task TestCreateCommentary()
    {
        List<Commentary> comms = [];
        var expectedComm = CreateMockCommentary();

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(default(Commentary)!);

        _mockCommentaryRepository.Setup(s => s.AddCommentary(It.IsAny<Commentary>()))
                                 .Callback((Commentary c) => comms.Add(c));

        await _commentaryService.CreateCommentary(expectedComm);
        var actualComm = comms.Last();

        Assert.Equal(expectedComm, actualComm);
    }

    [Fact]
    public async Task TestCreateExistentCommentary()
    {
        var expectedComm = CreateMockCommentary();

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(expectedComm);

        async Task Action() => await _commentaryService.CreateCommentary(expectedComm);

        await Assert.ThrowsAsync<CommentaryExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateCommentary()
    {
        var expectedComm = CreateMockCommentary();
        expectedComm.Text = "very new comm";

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(expectedComm);

        _mockCommentaryRepository.Setup(s => s.UpdateCommentary(It.IsAny<Commentary>()))
                                 .ReturnsAsync(expectedComm);

        var actualComm = await _commentaryService.UpdateCommentary(expectedComm);

        Assert.Equal(actualComm.Text, expectedComm.Text);
    }

    [Fact]
    public async Task TestUpdateCommentaryNonexistent()
    {
        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(It.IsAny<Guid>()))
                                 .ReturnsAsync(default(Commentary)!);

        async Task Action() => await _commentaryService.UpdateCommentary(CreateMockCommentary());

        await Assert.ThrowsAsync<CommentaryNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetCommentaryById()
    {
        var expectedComm = CreateMockCommentary();

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(expectedComm);
        
        var actualComm = await _commentaryService.GetCommentaryById(expectedComm.Id);

        Assert.Equal(expectedComm, actualComm);
    }

    [Fact]
    public async Task TestGetCommentaryNonexistentById()
    {
        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(It.IsAny<Guid>()))
                                 .ReturnsAsync(default(Commentary)!);
        
        async Task Action() => await _commentaryService.GetCommentaryById(Guid.Empty);

        await Assert.ThrowsAsync<CommentaryNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetAudiofileCommentaries()
    {
        var audiofile = new Audiotrack(Guid.NewGuid(), "", 0.1f, Guid.NewGuid(), "");
        List<Commentary> expectedComms = [CreateMockCommentary()]; 

        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(audiofile.Id))
                                .ReturnsAsync(audiofile);
        
        _mockCommentaryRepository.Setup(s => s.GetAudiotrackCommentaries(audiofile.Id))
                                 .ReturnsAsync(expectedComms);

        var actualComms = await _commentaryService.GetAudiofileCommentaries(audiofile.Id);

        Assert.Equal(expectedComms, actualComms);
    }

    [Fact]
    public async Task TestGetAudiofileNonexistentCommentaries()
    {
        _mockAudiotrackRepository.Setup(s => s.GetAudiotrackById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiotrack)!);

        Task Action() => _commentaryService.GetAudiofileCommentaries(Guid.Empty);

        await Assert.ThrowsAsync<AudiotrackNotFoundException>(Action);
    }

    private static Commentary CreateMockCommentary()
    {
        return new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever1");
    }
}