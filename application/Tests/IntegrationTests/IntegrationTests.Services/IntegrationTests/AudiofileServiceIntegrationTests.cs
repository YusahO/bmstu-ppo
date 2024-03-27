using IntegrationTests.Services.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Services.AudiofileService;

namespace IntegrationTests.Services.IntegratonTests;

public class AudiofileServiceIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly IAudiofileService _audiofileService;

    public AudiofileServiceIntegrationTests()
    {
        _dbFixture = new();
        _audiofileService = new AudiofileService(_dbFixture.AudiofileRepository);
    }

    [Fact]
    public async Task TestGetAudiofileById()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofile = new Audiofile(audiofiles.First());

        var actualAudiofile = await _audiofileService.GetAudiofileById(expectedAudiofile.Id);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestGetAudiofileByIdNotFound()
    {
        Task Action() => _audiofileService.GetAudiofileById(Guid.Empty);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
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

        var actualAudiofile = await _audiofileService.UpdateAudiofile(expectedAudiofile);

        Assert.Equal(expectedAudiofile, actualAudiofile);
    }

    [Fact]
    public async Task TestUpdateAudiofileNonexistent()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofile = new Audiofile(Guid.Empty, "", 0.1f, Guid.Empty, "");

        Task Action() => _audiofileService.UpdateAudiofile(expectedAudiofile);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteAudiofile()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofileId = audiofiles.First().Id;
        await _audiofileService.DeleteAudiofile(expectedAudiofileId);

        Task Action() => _audiofileService.GetAudiofileById(expectedAudiofileId);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteAudiofileNonexistent()
    {
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();
        await _dbFixture.InsertAudiofiles(audiofiles);

        var expectedAudiofileId = Guid.Empty;

        Task Action() => _audiofileService.DeleteAudiofile(expectedAudiofileId);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    public void Dispose() => _dbFixture.Dispose();
}