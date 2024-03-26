using MewingPad.Common.Exceptions;
using MewingPad.Services.TagService;

namespace UnitTests.Services;

public class TagServiceUnitTest
{
    private readonly ITagService _tagService;
    private readonly Mock<ITagRepository> _mockTagRepository = new();
    private readonly Mock<IAudiofileRepository> _mockAudiofileRepository = new();

    public TagServiceUnitTest()
    {
        _tagService = new TagService(_mockTagRepository.Object,
                                     _mockAudiofileRepository.Object);
    }

    [Fact]
    public async Task TestCreateTag()
    {
        List<Tag> tags = [];
        var expectedTag = CreateMockTag();

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(default(Tag)!);

        _mockTagRepository.Setup(s => s.AddTag(It.IsAny<Tag>()))
                          .Callback((Tag t) => tags.Add(t));

        await _tagService.CreateTag(expectedTag);
        var actualTag = tags.Last();

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestCreateExistentTag()
    {
        var expectedTag = CreateMockTag();

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(expectedTag);

        async Task Action() => await _tagService.CreateTag(expectedTag);

        await Assert.ThrowsAsync<TagExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateTag()
    {
        var expectedTag = CreateMockTag();
        expectedTag.Name = "new tag name";

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(expectedTag);

        _mockTagRepository.Setup(s => s.UpdateTag(It.IsAny<Tag>()))
                          .ReturnsAsync(expectedTag);

        var actualTag = await _tagService.UpdateTagName(expectedTag.Id, expectedTag.Name);

        Assert.Equal(expectedTag.Name, actualTag.Name);
    }

    [Fact]
    public async Task TestUpdateTagNonexistent()
    {
        _mockTagRepository.Setup(s => s.GetTagById(It.IsAny<Guid>()))
                          .ReturnsAsync(default(Tag)!);

        async Task Action() => await _tagService.UpdateTagName(Guid.Empty, "");

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestDeleteTag()
    {
        List<Tag> tags = [CreateMockTag()];
        var expectedTag = new Tag(tags.First());

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(expectedTag);

        _mockTagRepository.Setup(s => s.DeleteTag(expectedTag.Id))
                          .Callback((Guid id) =>
                          {
                              tags.Remove(expectedTag);
                          });

        await _tagService.DeleteTag(expectedTag.Id);

        Assert.Empty(tags);
    }

    [Fact]
    public async Task TestDeleteTagNonexistent()
    {
        _mockTagRepository.Setup(s => s.GetTagById(It.IsAny<Guid>()))
                          .ReturnsAsync(default(Tag)!);

        async Task Action() => await _tagService.DeleteTag(Guid.Empty);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetTagById()
    {
        var expectedTag = CreateMockTag();

        _mockTagRepository.Setup(s => s.GetTagById(expectedTag.Id))
                          .ReturnsAsync(expectedTag);

        var actualTag = await _tagService.GetTagById(expectedTag.Id);

        Assert.Equal(expectedTag, actualTag);
    }

    [Fact]
    public async Task TestGetTagNonexistentById()
    {
        _mockTagRepository.Setup(s => s.GetTagById(It.IsAny<Guid>()))
                          .ReturnsAsync(default(Tag)!);

        async Task Action() => await _tagService.GetTagById(Guid.Empty);

        await Assert.ThrowsAsync<TagNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetAudiofileTags()
    {
        var audiofile = CreateMockAudiofile();
        List<Tag> expectedTags = [CreateMockTag()];

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(audiofile.Id))
                                .ReturnsAsync(audiofile);
        
        _mockTagRepository.Setup(s => s.GetAudiofileTags(audiofile.Id))
                          .ReturnsAsync(expectedTags);

        var actualTags = await _tagService.GetAudiofileTags(audiofile.Id);

        Assert.Equal(expectedTags, actualTags);
    }

    [Fact]
    public async Task TestGetAudiofileTagsEmpty()
    {
        var audiofile = CreateMockAudiofile();

        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(audiofile.Id))
                                .ReturnsAsync(audiofile);
        
        _mockTagRepository.Setup(s => s.GetAudiofileTags(audiofile.Id))
                          .ReturnsAsync([]);

        var actualTags = await _tagService.GetAudiofileTags(audiofile.Id);

        Assert.Empty(actualTags);
    }

   [Fact]
    public async Task TestGetAudiofileNonexstentTags()
    {
        _mockAudiofileRepository.Setup(s => s.GetAudiofileById(It.IsAny<Guid>()))
                                .ReturnsAsync(default(Audiofile)!);

        Task Action() => _tagService.GetAudiofileTags(Guid.Empty);

        await Assert.ThrowsAsync<AudiofileNotFoundException>(Action);
    }

    private static Tag CreateMockTag()
    {
        return new(Guid.NewGuid(), Guid.NewGuid(), "whatever1");
    }

    private static Audiofile CreateMockAudiofile()
    {
        return new(Guid.NewGuid(), "whatever", 10.3f, Guid.NewGuid(), "path/to/file");
    }
}