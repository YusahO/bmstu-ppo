using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class TagRepository(MewingPadDbContext context) : ITagRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<TagRepository>();

    public async Task AddTag(Tag tag)
    {
        _logger.Verbose("Entering AddTag method");

        try
        {
            await _context.Tags.AddAsync(TagConverter.CoreToDbModel(tag)!);
            await _context.SaveChangesAsync();
            _logger.Information($"Added tag (Id = {tag.Id}) to database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting AddTag method");
    }

    public async Task DeleteTag(Guid tagId)
    {
        _logger.Verbose("Entering DeleteTag method");

        var tagDbModel = await _context.Tags.FindAsync(tagId);
        try
        {
            _context.Tags.Remove(tagDbModel!);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occured", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteTag method");
    }

    public async Task<List<Tag>> GetAllTags()
    {
        _logger.Verbose("Entering GetAllTags method");

        var tags = await _context.Tags
            .Select(t => TagConverter.DbToCoreModel(t))
            .ToListAsync();
        if (tags.Count == 0)
        {
            _logger.Warning("Database has no entries of Tag");
        }
        
        _logger.Verbose("Exiting GetAllTags method");
        return tags;
    }

    public async Task<Tag?> GetTagById(Guid tagId)
    {
        _logger.Verbose("Entering GetTagById method");

        var tagDbModel = await _context.Tags.FindAsync(tagId);
        if (tagDbModel is null)
        {
            _logger.Warning($"Tag (Id = {tagId}) not found in database");
        }
        var tag = TagConverter.DbToCoreModel(tagDbModel);

        _logger.Verbose("Exiting GetTagById method");
        return tag;
    }

    public async Task<Tag> UpdateTag(Tag tag)
    {
        _logger.Verbose("Entering UpdateTag method");

        var tagDbModel = await _context.Tags.FindAsync(tag.Id);

        tagDbModel!.Id = tag.Id;
        tagDbModel!.AuthorId = tag.AuthorId;
        tagDbModel!.Name = tag.Name;

        await _context.SaveChangesAsync();
        _logger.Information($"Updated tag (Id = {tag.Id})");
        _logger.Verbose("Exiting UpdateTag method");
        return tag;
    }
}