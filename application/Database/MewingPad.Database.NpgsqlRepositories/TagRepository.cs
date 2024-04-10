using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.NpgsqlRepositories;

public class TagRepository(MewingPadDbContext context) : ITagRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddTag(Tag tag)
    {
        await _context.Tags.AddAsync(TagConverter.CoreToDbModel(tag)!);
        await _context.SaveChangesAsync();
    }

    // public async Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId)
    // {
    //     await _context.TagsAudiotracks.AddAsync(new(tagId, audiotrackId));
    //     await _context.SaveChangesAsync();
    // }

    public async Task DeleteTag(Guid tagId)
    {
        var tagDbModel = await _context.Tags.FindAsync(tagId);
        _context.Tags.Remove(tagDbModel!);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tag>> GetAllTags()
    {
        return await _context.Tags
            .Select(t => TagConverter.DbToCoreModel(t))
            .ToListAsync();
    }

    // public async Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagIds)
    // {
    //     var audiotracks = await _context.TagsAudiotracks
    //         .Where(ta => tagIds.Contains(ta.TagId))
    //         .Include(ta => ta.Audiotrack)
    //         .Select(ta => AudiotrackConverter.DbToCoreModel(ta.Audiotrack))
    //         .ToListAsync();
    //     return audiotracks!;
    // }

    // public async Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId)
    // {
    //     var tags = await _context.TagsAudiotracks
    //         .Where(ta => ta.AudiotrackId == audiotrackId)
    //         .Include(ta => ta.Tag)
    //         .Select(ta => TagConverter.DbToCoreModel(ta.Tag))
    //         .ToListAsync();
    //     return tags!;
    // }

    public async Task<Tag?> GetTagById(Guid tagId)
    {
        var tagDbModel = await _context.Tags.FindAsync(tagId);
        return TagConverter.DbToCoreModel(tagDbModel);
    }

    public async Task<Tag> UpdateTag(Tag tag)
    {
        var tagDbModel = await _context.Tags.FindAsync(tag.Id);

        tagDbModel!.Id = tag.Id;
        tagDbModel!.AuthorId = tag.AuthorId;
        tagDbModel!.Name = tag.Name;

        await _context.SaveChangesAsync();
        return TagConverter.DbToCoreModel(tagDbModel);
    }
}