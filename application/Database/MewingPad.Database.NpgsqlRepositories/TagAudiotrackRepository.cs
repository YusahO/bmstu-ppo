using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class TagAudiotrackRepository(MewingPadDbContext context) : ITagAudiotrackRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<TagAudiotrackRepository>();

    public async Task DeleteByTag(Guid tagId)
    {
        _logger.Verbose("Entering DeleteByTag method");

        var pairs = await _context.TagsAudiotracks
            .Where(ta => ta.TagId == tagId)
            .ToListAsync();
        if (pairs.Count == 0)
        {
            _logger.Warning($"Tag (Id = {tagId}) is not assigned to any Audiotrack");
            return;
        }

        try
        {
            _context.TagsAudiotracks.RemoveRange(pairs);
            await _context.SaveChangesAsync();
            foreach (var p in pairs)
            {
                _logger.Information($"Deleted {{ TagId = {p.TagId}, AudiotrackId = {p.AudiotrackId}}} from database");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteByTag method");
    }

    public async Task DeleteByAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteByTag method");

        var pairs = await _context.TagsAudiotracks
            .Where(ta => ta.TagId == audiotrackId)
            .ToListAsync();
        if (pairs.Count == 0)
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) has no tags");
            return;
        }

        try
        {
            _context.TagsAudiotracks.RemoveRange(pairs);
            await _context.SaveChangesAsync();
            foreach (var p in pairs)
            {
                _logger.Information($"Deleted {{ TagId = {p.TagId}, AudiotrackId = {p.AudiotrackId}}} from database");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteByTag method");
    }

    public async Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose("Entering AssignTagToAudiotrack method");

        try
        {
            await _context.TagsAudiotracks.AddAsync(new(tagId, audiotrackId));
            await _context.SaveChangesAsync();
            _logger.Information($"Assigned tag (Id = {tagId}) to audiotrack (Id = {audiotrackId})");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting AssignTagToAudiotrack method");
    }

    public async Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagIds)
    {
        _logger.Verbose("Entering GetAudiotracksWithTags method");

        var audiotracks = await _context.TagsAudiotracks
            .Where(ta => tagIds.Contains(ta.TagId))
            .Include(ta => ta.Audiotrack)
            .Select(ta => AudiotrackConverter.DbToCoreModel(ta.Audiotrack))
            .ToListAsync();
        if (audiotracks.Count == 0)
        {
            _logger.Warning("No audiotracks with tags {@Tags} found in database", tagIds);
        }

        _logger.Verbose("Exiting GetAudiotracksWithTags method");
        return audiotracks!;
    }

    public async Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackTags method");

        var tags = await _context.TagsAudiotracks
            .Where(ta => ta.AudiotrackId == audiotrackId)
            .Include(ta => ta.Tag)
            .Select(ta => TagConverter.DbToCoreModel(ta.Tag))
            .ToListAsync();
        if (tags.Count == 0)
        {
            _logger.Warning($"Audiotrack (Id = {audiotrackId}) has no tags");
        }

        _logger.Verbose("Exiting GetAudiotrackTags method");
        return tags!;
    }

    public async Task RemoveTagFromAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose("Entering RemoveTagFromAudiotrack method");

        try
        {
            _context.TagsAudiotracks.Remove(new(tagId, audiotrackId));
            await _context.SaveChangesAsync();
            _logger.Information($"Removed tag (Id = {tagId}) from audiotrack {audiotrackId}");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting RemoveTagFromAudiotrack method");

    }
}