using MewingPad.Common.Exceptions;
using MewingPad.Services.CommentaryService;

namespace UnitTests.Services;

public class CommentaryServiceUnitTest
{
    private readonly ICommentaryService _commentaryService;
    private readonly Mock<ICommentaryRepository> _mockCommentaryRepository = new();

    public CommentaryServiceUnitTest()
    {
        _commentaryService = new CommentaryService(_mockCommentaryRepository.Object);
    }

    [Fact]
    public async Task TestCreateCommentary()
    {
        var comms = CreateMockCommentaries();
        var expectedComm = new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "new comm");

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(comms.Find(f => f.Id == expectedComm.Id)!);

        _mockCommentaryRepository.Setup(s => s.AddCommentary(It.IsAny<Commentary>()))
                                 .Callback((Commentary c) => comms.Add(c));

        await _commentaryService.CreateCommentary(expectedComm);
        var actualComm = comms.Last();

        Assert.Equal(expectedComm, actualComm);
    }

    [Fact]
    public async Task TestCreateExistentCommentary()
    {
        var comms = CreateMockCommentaries();
        var expectedComm = comms[1];

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(comms.Find(f => f.Id == expectedComm.Id)!);

        async Task Action() => await _commentaryService.CreateCommentary(expectedComm);

        await Assert.ThrowsAsync<CommentaryExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateCommentary()
    {
        var expectedComm = CreateMockCommentaries()[0];
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
        var comms = CreateMockCommentaries();
        var expectedComm = new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "new comm");

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(comms.Find(f => f.Id == expectedComm.Id)!);

        async Task Action() => await _commentaryService.UpdateCommentary(expectedComm);

        await Assert.ThrowsAsync<CommentaryNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetCommentaryById()
    {
        var comms = CreateMockCommentaries();
        var expectedComm = comms[0];

        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(comms.Find(f => f.Id == expectedComm.Id)!);
        
        var comm = await _commentaryService.GetCommentaryById(expectedComm.Id);

        Assert.Equal(expectedComm, comm);
    }

    [Fact]
    public async Task TestGetCommentaryNonexistentById()
    {
        var comms = CreateMockCommentaries();
        var expectedComm = new Commentary(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "new comm");
        
        _mockCommentaryRepository.Setup(s => s.GetCommentaryById(expectedComm.Id))
                                 .ReturnsAsync(comms.Find(f => f.Id == expectedComm.Id)!);
        
        async Task Action() => await _commentaryService.GetCommentaryById(expectedComm.Id);

        await Assert.ThrowsAsync<CommentaryNotFoundException>(Action);
    }

    private static List<Commentary> CreateMockCommentaries()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever1"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever2"),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever3"),
        ];
    }
}