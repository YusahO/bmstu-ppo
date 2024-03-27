using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class CommentaryRepositoryIntegrationTests() : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreateCommentary()
    {
        var expectedCommentary = new Commentary(Guid.NewGuid(), Guid.Empty, Guid.Empty, "");
        await _dbFixture.CommentaryRepository.AddCommentary(expectedCommentary);

        var actualCommentary = await _dbFixture.CommentaryRepository.GetCommentaryById(expectedCommentary.Id);

        Assert.Equal(expectedCommentary, actualCommentary);
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

        var actualCommentary = await _dbFixture.CommentaryRepository.UpdateCommentary(expectedCommentary);

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

        var actualCommentaries = await _dbFixture.CommentaryRepository.GetAudiofileCommentaries(expectedAudiofile.Id);

        Assert.Equal(expectedCommentaries, actualCommentaries);
    }

    public void Dispose() => _dbFixture.Dispose();
}