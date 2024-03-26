﻿using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.TagService;

public class TagService(ITagRepository tagRepository, IAudiofileRepository audiofileRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IAudiofileRepository _audiofileRepository = audiofileRepository;

    public async Task CreateTag(Tag tag)
    {
        if (await _tagRepository.GetTagById(tag.Id) is not null)
        {
            throw new TagExistsException(tag.Id);
        }
        await _tagRepository.AddTag(tag);
    }

    public async Task<Tag> UpdateTagName(Guid tagId, string tagName)
    {
        var tag = await _tagRepository.GetTagById(tagId);
        if (tag is null)
        {
            throw new TagNotFoundException(tagId);
        }
        tag.Name = tagName;
        return await _tagRepository.UpdateTag(tag);
    }

    public async Task DeleteTag(Guid tagId)
    {
        if (await _tagRepository.GetTagById(tagId) is null)
        {
            throw new TagNotFoundException(tagId);
        }
        await _tagRepository.DeleteTag(tagId);
    }

    public async Task<Tag> GetTagById(Guid tagId)
    {
        var tag = await _tagRepository.GetTagById(tagId)
                  ?? throw new TagNotFoundException(tagId);
        return tag;
    }

    public async Task<List<Tag>> GetAudiofileTags(Guid audiofileId)
    {
        if (await _audiofileRepository.GetAudiofileById(audiofileId) is null)
        {
            throw new AudiofileNotFoundException(audiofileId);
        }
        return await _tagRepository.GetAudiofileTags(audiofileId);
    }
}
