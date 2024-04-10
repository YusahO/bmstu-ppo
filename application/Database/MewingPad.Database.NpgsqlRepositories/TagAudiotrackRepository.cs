using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.NpgsqlRepositories;

public class TagAudiotrackRepository(MewingPadDbContext context) : ITagAudiotrackRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task DeleteByTag(Guid tagId)
    {
        var pairs = await _context.TagsAudiotracks
            .Where(ta => ta.TagId == tagId)
            .ToListAsync();
        
        if (pairs.Count == 0)
        {
            return;
        }
        
        _context.TagsAudiotracks.RemoveRange(pairs);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByAudiotrack(Guid audiotrackId)
    {
        var pairs = await _context.TagsAudiotracks
            .Where(ta => ta.TagId == audiotrackId)
            .ToListAsync();
        
        if (pairs.Count == 0)
        {
            return;
        }
        
        _context.TagsAudiotracks.RemoveRange(pairs);
        await _context.SaveChangesAsync();
    }

    public async Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId)
    {
        await _context.TagsAudiotracks.AddAsync(new(tagId, audiotrackId));
        await _context.SaveChangesAsync();
    }

    public async Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagIds)
    {
        var audiotracks = await _context.TagsAudiotracks
            .Where(ta => tagIds.Contains(ta.TagId))
            .Include(ta => ta.Audiotrack)
            .Select(ta => AudiotrackConverter.DbToCoreModel(ta.Audiotrack))
            .ToListAsync();
        return audiotracks!;
    }

    public async Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId)
    {
        var tags = await _context.TagsAudiotracks
            .Where(ta => ta.AudiotrackId == audiotrackId)
            .Include(ta => ta.Tag)
            .Select(ta => TagConverter.DbToCoreModel(ta.Tag))
            .ToListAsync();
        return tags!;
    }

    public async Task RemoveTagFromAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _context.TagsAudiotracks.Remove(new(tagId, audiotrackId));
        await _context.SaveChangesAsync();
    }
}