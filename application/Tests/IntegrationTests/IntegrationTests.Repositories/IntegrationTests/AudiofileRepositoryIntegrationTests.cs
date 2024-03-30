using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class AudiofileRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly IAudiofileRepository _audiofileRepository;

    public AudiofileRepositoryIntegrationTests()
    {
        _audiofileRepository = new AudiofileRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateAudiofile()
    {
        var expectedAudiofile = new Audiofile(Guid.NewGuid(), "", 0.1f, Guid.Empty, "path/to/file");
        await _audiofileRepository.AddAudiofile(expectedAudiofile);

        var actualAudiofile = await _dbFixture.GetAudiofileById(expectedAudiofile.Id);

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

        var actualAudiofile = await _audiofileRepository.UpdateAudiofile(expectedAudiofile);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofileId = audiofiles.First().Id;
        await _audiofileRepository.DeleteAudiofile(expectedAudiofileId);

        var actualAudiofile = await _dbFixture.GetAudiofileById(expectedAudiofileId);

        Assert.Null(actualAudiofile);
    }

    public void Dispose() => _dbFixture.Dispose();
}