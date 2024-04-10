using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class CommentaryRepository(MewingPadDbContext context) : ICommentaryRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<CommentaryRepository>();

    public async Task AddCommentary(Commentary commentary)
    {
        _logger.Verbose("Entering AddCommentary method");

        try
        {
            await _context.Commentaries.AddAsync(CommentaryConverter.CoreToDbModel(commentary)!);
            await _context.SaveChangesAsync();
            _logger.Information($"Added commentary (Id = {commentary.Id}) to database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting AddCommentary method");
    }

    public async Task DeleteCommentary(Guid commentaryId)
    {
        _logger.Verbose("Entering DeleteCommentary method");

        try
        {
            var commentaryDbModel = await _context.Commentaries.FindAsync(commentaryId);
            _context.Commentaries.Remove(commentaryDbModel!);
            await _context.SaveChangesAsync();
            _logger.Information($"Deleted commentary (Id = {commentaryId}) from database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting DeleteCommentary method");
    }

    public async Task<List<Commentary>> GetAudiotrackCommentaries(Guid audiotrackId)
    {
        _logger.Verbose("Entering GetAudiotrackCommentaries method");

        var commentaries = await _context.Commentaries
            .Where(c => c.AudiotrackId == audiotrackId)
            .Select(c => CommentaryConverter.DbToCoreModel(c))
            .ToListAsync();
        if (commentaries.Count == 0)
        {
            _logger.Warning($"Database contains no commentaries for audiotrack with id {audiotrackId}");
        }

        _logger.Verbose("Exiting GetAudiotrackCommentaries method");
        return commentaries;
    }

    public async Task<Commentary?> GetCommentaryById(Guid commentaryId)
    {
        _logger.Verbose("Entering GetCommentaryById method");

        var commentaryDbModel = await _context.Commentaries.FindAsync(commentaryId);
        if (commentaryDbModel is null)
        {
            _logger.Warning($"Commentary (Id = {commentaryId}) not found in database");
        }
        var commentary = CommentaryConverter.DbToCoreModel(commentaryDbModel);

        _logger.Verbose("Exiting GetCommentaryById method");
        return commentary;
    }

    public async Task<Commentary> UpdateCommentary(Commentary commentary)
    {
        _logger.Verbose("Entering UpdateCommentary method");

        var commentaryDbModel = await _context.Commentaries.FindAsync(commentary.Id);

        commentaryDbModel!.Id = commentary.Id;
        commentaryDbModel!.AuthorId = commentary.AuthorId;
        commentaryDbModel!.AudiotrackId = commentary.AudiotrackId;
        commentaryDbModel!.Text = commentary.Text;

        await _context.SaveChangesAsync();
        _logger.Information($"Updated commentary (Id = {commentary.Id})");
        _logger.Verbose("Exiting UpdateCommentary method");
        return commentary;
    }
}