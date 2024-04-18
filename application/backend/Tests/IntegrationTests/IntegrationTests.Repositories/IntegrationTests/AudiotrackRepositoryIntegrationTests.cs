using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class AudiotrackRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly IAudiotrackRepository _audiotrackRepository;

    public AudiotrackRepositoryIntegrationTests()
    {
        _audiotrackRepository = new AudiotrackRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateAudiofile()
    {
        var expectedAudiofile = new Audiotrack(Guid.NewGuid(), "", 0.1f, Guid.Empty, "path/to/file");
        await _audiotrackRepository.AddAudiotrack(expectedAudiofile);

        var actualAudiofile = await _dbFixture.GetAudiotrackById(expectedAudiofile.Id);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestUpdateAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();
        await _dbFixture.InsertAudiotracks(audiofiles);

        var expectedAudiofile = new Audiotrack(audiofiles.First())
        {
            Duration = 13.4f
        };

        var actualAudiofile = await _audiotrackRepository.UpdateAudiotrack(expectedAudiofile);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();
        await _dbFixture.InsertAudiotracks(audiofiles);

        var expectedAudiofileId = audiofiles.First().Id;
        await _audiotrackRepository.DeleteAudiotrack(expectedAudiofileId);

        var actualAudiofile = await _dbFixture.GetAudiotrackById(expectedAudiofileId);

        Assert.Null(actualAudiofile);
    }

    public void Dispose() => _dbFixture.Dispose();
}