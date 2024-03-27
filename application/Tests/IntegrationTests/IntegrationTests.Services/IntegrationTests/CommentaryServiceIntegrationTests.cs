using IntegrationTests.Services.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Services.CommentaryService;

namespace IntegrationTests.Services.IntegratonTests;

public class CommentaryServiceIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly ICommentaryService _commentaryService;

    public CommentaryServiceIntegrationTests()
    {
        _dbFixture = new();
        _commentaryService = new CommentaryService(_dbFixture.CommentaryRepository,
                                                   _dbFixture.AudiofileRepository);
    }

    [Fact]
    public async Task TestGetCommentaryById()
    {
        var commentaries = InMemoryDbFixture.CreateMockCommentaries();
        await _dbFixture.InsertCommentaries(commentaries);

        var expectedCommentary = new Commentary(commentaries.First());

        var actualCommentary = await _commentaryService.GetCommentaryById(expectedCommentary.Id);

        Assert.Equal(expectedCommentary, actualCommentary);
    }

    [Fact]
    public async Task TestGetCommentaryByIdNotFound()
    {
        Task Action() => _commentaryService.GetCommentaryById(Guid.Empty);

        await Assert.ThrowsAsync<CommentaryNotFoundException>(Action);
    }

    [Fact]
    public async Task TestUpdateCommentary()
    {
        var commentaries = InMemoryDbFixture.CreateMockCommentaries();
        await _dbFixture.InsertCommentaries(commentaries);

        var expectedCommentary = new Commentary(commentaries.First())
        {
            Text = "new text"
        };

        var actualCommentary = await _commentaryService.UpdateCommentary(expectedCommentary);

        Assert.Equal(expectedCommentary, actualCommentary);
    }

    [Fact]
    public async Task TestGetAudiofileCommentaries()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        var expectedAudiofile = audiofiles.First();

        var expectedCommentaries = InMemoryDbFixture.CreateMockCommentaries();
        foreach (var comm in expectedCommentaries)
        {
            comm.AudiofileId = expectedAudiofile.Id;
        }

        await _dbFixture.InsertAudiofiles(audiofiles);
        await _dbFixture.InsertCommentaries(expectedCommentaries);

        var actualCommentaries = await _commentaryService.GetAudiofileCommentaries(expectedAudiofile.Id);

        Assert.Equal(expectedCommentaries, actualCommentaries);
    }

    [Fact]
    public async Task TestGetAudiofileCommentariesEmpty()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        var expectedAudiofile = audiofiles.First();

        await _dbFixture.InsertAudiofiles(audiofiles);
        await _dbFixture.InsertCommentaries(InMemoryDbFixture.CreateMockCommentaries());

        var actualCommentaries = await _commentaryService.GetAudiofileCommentaries(expectedAudiofile.Id);

        Assert.Empty(actualCommentaries);
    }

    [Fact]
    public async Task TestGetAudiofileNonexistentCommentaries()
    {
        await _dbFixture.InsertCommentaries(InMemoryDbFixture.CreateMockCommentaries());

        Task Action() => _commentaryService.GetAudiofileCommentaries(Guid.Empty);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    public void Dispose() => _dbFixture.Dispose();
}