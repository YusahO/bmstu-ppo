using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using Serilog;

namespace MewingPad.Services.TagService;

public class TagService(ITagRepository tagRepository,
                        IAudiotrackRepository audiofileRepository,
                        ITagAudiotrackRepository tagAudiotrackRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IAudiotrackRepository _audiofileRepository = audiofileRepository;
    private readonly ITagAudiotrackRepository _tagAudiotrackRepository = tagAudiotrackRepository;

    private readonly ILogger _logger = Log.ForContext<TagService>();

    public async Task CreateTag(Tag tag)
    {
        _logger.Verbose("Entering CreateTag method");

        if (await _tagRepository.GetTagById(tag.Id) is not null)
        {
            _logger.Error($"Tag (Id = {tag.Id}) already exists");
            throw new TagExistsException(tag.Id);
        }
        await _tagRepository.AddTag(tag);

        _logger.Verbose("Exiting CreateTag method");
    }

    public async Task<Tag> UpdateTagName(Guid tagId, string tagName)
    {
        _logger.Verbose("Entering UpdateTagName method");

        var tag = await _tagRepository.GetTagById(tagId);
        if (tag is null)
        {
            _logger.Error($"Tag (Id = {tagId}) not found");
            throw new TagNotFoundException(tagId);
        }
        tag.Name = tagName;
        await _tagRepository.UpdateTag(tag);

        _logger.Verbose("Exiting UpdateTagName method");
        return tag;
    }

    public async Task DeleteTag(Guid tagId)
    {
        _logger.Verbose("Entering DeleteTag method");

        if (await _tagRepository.GetTagById(tagId) is null)
        {
            _logger.Error($"Tag (Id = {tagId}) not found");
            throw new TagNotFoundException(tagId);
        }
        await _tagAudiotrackRepository.DeleteByTag(tagId);
        await _tagRepository.DeleteTag(tagId);

        _logger.Verbose("Exiting DeleteTag method");
    }

    public async Task<Tag> GetTagById(Guid tagId)
    {
        _logger.Verbose("Entering GetTagById method");

        var tag = await _tagRepository.GetTagById(tagId);
        if (tag is null)
        {
            _logger.Error($"Tag (Id = {tagId}) not found");
            throw new TagNotFoundException(tagId);
        }

        _logger.Verbose("Exiting GetTagById method");
        return tag;
    }

    public async Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackTags method");

        if (await _audiofileRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        var tags = await _tagAudiotrackRepository.GetAudiotrackTags(audiotrackId);

        _logger.Verbose("Exiting GetAudiotrackTags method");
        return tags;
    }

    public async Task<List<Tag>> GetAllTags()
    {
        _logger.Verbose("Entering GetAllTags method");
        var tags = await _tagRepository.GetAllTags();
        _logger.Verbose("Exiting GetAllTags method");
        return tags;
    }

    public async Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagIds)
    {
        _logger.Verbose("Entering GetAudiotracksWithTags method");
        foreach (var tid in tagIds)
        {
            if (await _tagRepository.GetTagById(tid) is null)
            {
                _logger.Error($"Tag (Id = {tid}) not found");
                throw new TagNotFoundException(tid);
            }
        }
        var audios = await _tagAudiotrackRepository.GetAudiotracksWithTags(tagIds);

        _logger.Verbose("Exiting GetAudiotracksWithTags method");
        return audios;
    }

    public async Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose("Entering AssignTagToAudiotrack method");

        if (await _tagRepository.GetTagById(tagId) is null)
        {
            _logger.Error($"Tag (Id = {tagId}) not found");
            throw new TagNotFoundException(tagId);
        }
        if (await _audiofileRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        await _tagAudiotrackRepository.AssignTagToAudiotrack(audiotrackId, tagId);

        _logger.Verbose("Exiting AssignTagToAudiotrack method");
    }

    public async Task DeleteTagFromAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose("Entering DeleteTagFromAudiotrack method");

        if (await _tagRepository.GetTagById(tagId) is null)
        {
            _logger.Error($"Tag (Id = {tagId}) not found");
            throw new TagNotFoundException(tagId);
        }
        if (await _audiofileRepository.GetAudiotrackById(audiotrackId) is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        await _tagAudiotrackRepository.RemoveTagFromAudiotrack(audiotrackId, tagId);

        _logger.Verbose("Exiting DeleteTagFromAudiotrack method");
    }
}
