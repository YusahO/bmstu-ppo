using MewingPad.Common.Exceptions;
using MewingPad.Services.TagService;

namespace UnitTests.Services;

public class TagServiceUnitTest
{
    private readonly ITagService _tagService;
    private readonly Mock<ITagRepository> _mockTagRepository = new();

    public TagServiceUnitTest()
    {
        _tagService = new TagService(_mockTagRepository.Object);
    }

    [Fact]
    public async Task TestCreateTag()
    {
        var tags = CreateMockTags();
        var expectedTag = new Tag(Guid.NewGuid(), Guid.NewGuid(), "tag name");

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        _mockTagRepository.Setup(s => s.AddTag(It.IsAny<Tag>()))
                          .Callback((Tag t) => tags.Add(t));

        await _tagService.CreateTag(expectedTag);
        var actualComm = tags.Last();

        Assert.Equal(expectedTag, actualComm);
    }

    [Fact]
    public async Task TestCreateExistentTag()
    {
        var tags = CreateMockTags();
        var expectedTag = tags[1];

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        async Task Action() => await _tagService.CreateTag(expectedTag);

        await Assert.ThrowsAsync<TagExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateTag()
    {
        var tags = CreateMockTags();
        var expectedTag = new Tag(tags[1]);
        var expectedTagName = "new tag name";

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                                 .ReturnsAsync(expectedTag);

        _mockTagRepository.Setup(s => s.UpdateTag(It.IsAny<Tag>()))
                          .ReturnsAsync(expectedTag);

        var actual = await _tagService.UpdateTagName(expectedTag.Id, expectedTagName);

        Assert.Equal(expectedTagName, actual.Name);
    }

    [Fact]
    public async Task TestUpdateTagNonexistent()
    {
        var tags = CreateMockTags();
        var expectedTag = new Tag(Guid.NewGuid(), Guid.NewGuid(), "tag name");
        var expectedTagName = "new tag name";

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        async Task Action() => await _tagService.UpdateTagName(expectedTag.Id, expectedTagName);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteTag()
    {
        var tags = CreateMockTags();
        var expectedTag = tags[1];

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        _mockTagRepository.Setup(s => s.DeleteTag(It.IsAny<Guid>()))
                          .Callback((Guid id) =>
                          {
                              tags.Remove(tags.Find(t => t.Id == expectedTag.Id)!);
                          });

        await _tagService.DeleteTag(expectedTag.Id);
        var actualTag = tags[1];

        Assert.Equal(2, tags.Count);
    }

    [Fact]
    public async Task TestDeleteTagNonexistent()
    {
        var tags = CreateMockTags();
        var expectedTag = new Tag(Guid.NewGuid(), Guid.NewGuid(), "tag name");

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        async Task Action() => await _tagService.DeleteTag(expectedTag.Id);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetTagById()
    {
        var tags = CreateMockTags();
        var expectedTag = tags[0];

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(t => t.Id == expectedTag.Id)!);

        var tag = await _tagService.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, tag);
    }

    [Fact]
    public async Task TestGetTagNonexistentById()
    {
        var tags = CreateMockTags();
        var expectedTag = new Tag(Guid.NewGuid(), Guid.NewGuid(), "new tag");

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(tags.Find(f => f.Id == expectedTag.Id)!);

        async Task Action() => await _tagService.GetTagById(expectedTag.Id);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    private static List<Tag> CreateMockTags()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), "whatever1"),
            new(Guid.NewGuid(), Guid.NewGuid(), "whatever2"),
            new(Guid.NewGuid(), Guid.NewGuid(), "whatever3"),
        ];
    }
}