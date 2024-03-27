using MewingPad.Services.TagService;
using IntegrationTests.Services.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Services.IntegratonTests;

public class TagServiceIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture;
    private readonly ITagService _tagService;

    public TagServiceIntegrationTests()
    {
        _dbFixture = new();
        _tagService = new TagService(_dbFixture.TagRepository,
                                     _dbFixture.AudiofileRepository);
    }

    [Fact]
    public async Task TestGetTagById()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTag = new Tag(tags.First());

        var actualTag = await _tagService.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetTagByIdNotFound()
    {
        Task Action() => _tagService.GetTagById(Guid.Empty);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestUpdateTagName()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTag = new Tag(tags.First())
        {
            Name = "new name"
        };

        var actualTag = await _tagService.UpdateTagName(expectedTag.Id, expectedTag.Name);

        Assert.Equal(expectedTag.Id, actualTag.Id);
        Assert.Equal(expectedTag.AuthorId, actualTag.AuthorId);
        Assert.Equal(expectedTag.Name, actualTag.Name);
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

        var actualTags = await _tagService.GetAudiofileTags(expectedAudiofile.Id);

        Assert.Equal([expectedTag], actualTags);
    }

    [Fact]
    public async Task TestGetAudiofileTagsEmpty()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        var audiofiles = InMemoryDbFixture.CreateMockAudiofiles();

        var expectedAudiofile = audiofiles.First();

        await _dbFixture.InsertTags(tags);
        await _dbFixture.InsertAudiofiles(audiofiles);

        var actualTags = await _tagService.GetAudiofileTags(expectedAudiofile.Id);

        Assert.Empty(actualTags);
    }

    [Fact]
    public async Task TestDeleteTag()
    {
        var tags = InMemoryDbFixture.CreateMockTags();
        await _dbFixture.InsertTags(tags);

        var expectedTagId = tags.Last().Id;
        await _tagService.DeleteTag(expectedTagId);

        Task Action() => _tagService.GetTagById(expectedTagId);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    public void Dispose() => _dbFixture.Dispose();
}