using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.CommentaryService;

public class CommentaryService(ICommentaryRepository commentaryRepository) : ICommentaryService
{
    private readonly ICommentaryRepository _commentaryRepository = commentaryRepository;

    public async Task CreateCommentary(Commentary commentary)
    {
        if (await _commentaryRepository.GetCommentaryById(commentary.Id) is not null)
        {
            throw new CommentaryExistsException(commentary.Id);
        }
        await _commentaryRepository.AddCommentary(commentary);
    }

    public async Task<Commentary> GetCommentaryById(Guid commentaryId)
    {
        var commentary = await _commentaryRepository.GetCommentaryById(commentaryId)
                         ?? throw new CommentaryNotFoundException(commentaryId);
        return commentary;
    }

    public async Task<Commentary> UpdateCommentary(Commentary commentary)
    {
        if (await _commentaryRepository.GetCommentaryById(commentary.Id) is null)
        {
            throw new CommentaryNotFoundException(commentary.Id);
        }
        return await _commentaryRepository.UpdateCommentary(commentary);
    }
}
