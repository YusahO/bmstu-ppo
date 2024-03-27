using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class AudiofileRepositoryIntegrationTests() : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreateAudiofile()
    {
        var expectedAudiofile = new Audiofile(Guid.NewGuid(), "", 0.1f, Guid.Empty, "path/to/file");
        await _dbFixture.AudiofileRepository.AddAudiofile(expectedAudiofile);

        var actualAudiofile = await _dbFixture.AudiofileRepository.GetAudiofileById(expectedAudiofile.Id);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestUpdateAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofile = new Audiofile(audiofiles.First())
        {
            Duration = 13.4f
        };

        var actualAudiofile = await _dbFixture.AudiofileRepository.UpdateAudiofile(expectedAudiofile);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofileId = audiofiles.First().Id;
        await _dbFixture.AudiofileRepository.DeleteAudiofile(expectedAudiofileId);

        var actualAudiofile = await _dbFixture.AudiofileRepository.GetAudiofileById(expectedAudiofileId);

        Assert.Null(actualAudiofile);
    }

    public void Dispose() => _dbFixture.Dispose();
}