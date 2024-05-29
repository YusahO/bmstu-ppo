using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.MongoDB.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.MongoDB.Repositories;

public class TagAudiotrackRepository(MewingPadMongoDbContext context) : ITagAudiotrackRepository
{
    private readonly MewingPadMongoDbContext _context = context;
    private readonly ILogger _logger = Log.ForContext<AudiotrackRepository>();

    public async Task DeleteByTag(Guid tagId)
    {
        _logger.Verbose("Entering DeleteByTag");

        try
        {
            var audios = await _context.Audiotracks
                .Where(a => a.TagIds.Contains(tagId))
                .ToListAsync();
            for (int i = 0; i < audios.Count; ++i)
            {
                audios[i].TagIds.Remove(tagId);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting DeleteByTag");
    }

    public Task DeleteByAudiotrack(Guid audiotrackId)
    {
        _logger.Verbose("Entering DeleteByTag");
        
        _logger.Information("Nothing to do in MongoDB");

        _logger.Verbose("Exiting DeleteByTag");
        return Task.CompletedTask;
    }

    public async Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose($"Entering AssignTagToAudiotrack({audiotrackId}, {tagId})");

        try
        {
            var foundAudio = await _context.Audiotracks
                .SingleAsync(a => a.Id == audiotrackId);
            foundAudio.TagIds.Add(tagId);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AssignTagToAudiotrack");
    }

    public async Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagIds)
    {
        _logger.Verbose("Entering GetAudiotracksWithTags({Ids})", tagIds);

        List<Audiotrack> audiotracks;
        try
        {
            var found = await _context.Audiotracks
                .Where(a => a.TagIds.Any(atid => tagIds.Any(tid => atid == tid)))
                .Distinct()
                .ToListAsync();
            audiotracks = found.Select(a => AudiotrackConverter.DbToCoreModel(a)).ToList();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAudiotracksWithTags");
        return audiotracks;
    }

    public async Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId)
    {
        _logger.Verbose($"Entering GetAudiotrackTags({audiotrackId})");

        List<Tag?> tags = [];
        try
        {
            var foundIds = await _context.Audiotracks
                .Where(a => a.Id == audiotrackId)
                .Select(a => a.TagIds)
                .FirstAsync();
            
            foreach (var tid in foundIds)
            {
                var tag = _context.Tags.Single(t => t.Id == tid);
                tags.Add(TagConverter.DbToCoreModel(tag));
            }
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAudiotrackTags");
        return tags!;
    }

    public async Task RemoveTagFromAudiotrack(Guid audiotrackId, Guid tagId)
    {
        _logger.Verbose($"Entering RemoveTagFromAudiotrack({audiotrackId}, {tagId})");

        try
        {
            var audio = await _context.Audiotracks
                .SingleAsync(a => a.Id == audiotrackId);
            audio.TagIds.Remove(tagId);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting RemoveTagFromAudiotrack");
    }
}