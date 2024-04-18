using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class TagRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly ITagRepository _tagRepository;

    public TagRepositoryIntegrationTests()
    {
        _tagRepository = new TagRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateTag()
    {
        var expectedTag = new Tag(Guid.NewGuid(), Guid.Empty, "name");
        
        await _tagRepository.AddTag(expectedTag);
        var actualTag = await _dbFixture.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetTagById()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTag = new Tag(tags.First());

        var actualTag = await _tagRepository.GetTagById(expectedTag.Id);

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

        var actualTag = await _tagRepository.UpdateTag(expectedTag);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetAudiofileTags()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        var audiofiles = InMemoryDbFixture.CreateMockAudiotracks();

        var expectedTag = tags.First();
        var expectedAudiofile = audiofiles.First();
        List<KeyValuePair<Guid, Guid>> pairs = [
            new(expectedTag.Id, expectedAudiofile.Id)];

        await _dbFixture.InsertTags(tags);
        await _dbFixture.InsertAudiotracks(audiofiles);
        await _dbFixture.InsertTagsAudiotracks(pairs);

        var actualTags = await _tagRepository.GetAudiotrackTags(expectedAudiofile.Id);

        Assert.Equal([expectedTag], actualTags);
    }

    [Fact]
    public async Task TestDeleteTag()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTagId = tags.Last().Id;
        await _tagRepository.DeleteTag(expectedTagId);

        var actualTag = await _dbFixture.GetTagById(expectedTagId);

        Assert.Null(actualTag);
    }

    public void Dispose() => _dbFixture.Dispose();
}