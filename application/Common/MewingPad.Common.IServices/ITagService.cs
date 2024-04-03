using MewingPad.Common.Entities;
namespace MewingPad.Services.TagService;

public interface ITagService
{
    Task CreateTag(Tag tag);
    Task<Tag> UpdateTagName(Guid tagId, string tagName);
    Task DeleteTag(Guid tagId);
    Task<Tag> GetTagById(Guid tagId);
    Task<List<Tag>> GetAllTags();
    Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId);
    Task<List<Audiotrack>> GetAudiotracksWithTag(Guid tagId);
}
