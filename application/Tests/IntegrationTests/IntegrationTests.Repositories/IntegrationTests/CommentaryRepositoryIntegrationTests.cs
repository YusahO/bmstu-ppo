using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class CommentaryRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly ICommentaryRepository _commentaryRepository;

    public CommentaryRepositoryIntegrationTests()
    {
        _commentaryRepository = new CommentaryRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateCommentary()
    {
        var expectedCommentary = new Commentary(Guid.NewGuid(), Guid.Empty, Guid.Empty, "");
        await _commentaryRepository.AddCommentary(expectedCommentary);

        var actualCommentary = await _dbFixture.GetCommentaryById(expectedCommentary.Id);

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

        var actualCommentary = await _commentaryRepository.UpdateCommentary(expectedCommentary);

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

        var actualCommentaries = await _commentaryRepository.GetAudiofileCommentaries(expectedAudiofile.Id);

        Assert.Equal(expectedCommentaries.OrderBy(e => e.Id),
                     actualCommentaries.OrderBy(a => a.Id));
    }

    public void Dispose() => _dbFixture.Dispose();
}