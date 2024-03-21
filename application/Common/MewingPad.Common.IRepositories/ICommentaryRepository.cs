using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface ICommentaryRepository
{
    Task AddCommentary(Commentary commentary);
    Task<Commentary> UpdateCommentary(Commentary commentary);
    Task<Commentary> GetCommentaryById(Guid commentaryId);
}