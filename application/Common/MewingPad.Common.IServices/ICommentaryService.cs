using MewingPad.Common.Entities;

namespace MewingPad.Services.CommentaryService;

public interface ICommentaryService
{
    Task CreateCommentary(Commentary commentary);
    Task<Commentary> UpdateCommentary(Commentary commentary);
    Task<Commentary> GetCommentaryById(Guid commentaryId);
}