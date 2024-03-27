using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class TagRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreateTag()
    {
        var expectedTag = new Tag(Guid.NewGuid(), Guid.Empty, "name");
        
        await _dbFixture.TagRepository.AddTag(expectedTag);
        var actualTag = await _dbFixture.TagRepository.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetTagById()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTag = new Tag(tags.First());

        var actualTag = await _dbFixture.TagRepository.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestUpdateTag()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTag = new Tag(tags.First())
        {
            Name = "new name"
        };

        var actualTag = await _dbFixture.TagRepository.UpdateTag(expectedTag);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetAudiofileTags()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();

        var expectedTag = tags.First();
        var expectedAudiofile = audiofiles.First();
        List<KeyValuePair<Guid, Guid>> pairs = [
            new(expectedTag.Id, expectedAudiofile.Id)];

        await _dbFixture.InsertTags(tags);
        await _dbFixture.InsertAudiofiles(audiofiles);
        await _dbFixture.InsertTagsAudiofiles(pairs);

        var actualTags = await _dbFixture.TagRepository.GetAudiofileTags(expectedAudiofile.Id);

        Assert.Equal([expectedTag], actualTags);
    }

    [Fact]
    public async Task TestDeleteTag()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTagId = tags.Last().Id;
        await _dbFixture.TagRepository.DeleteTag(expectedTagId);

        var actualTag = await _dbFixture.TagRepository.GetTagById(expectedTagId);

        Assert.Null(actualTag);
    }

    public void Dispose() => _dbFixture.Dispose();
}