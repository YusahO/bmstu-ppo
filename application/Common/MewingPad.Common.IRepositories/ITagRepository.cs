using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface ITagRepository
{
    Task AddTag(Tag tag);
    Task<Tag> UpdateTag(Tag tag);
    Task DeleteTag(Guid tagId);
    Task<Tag?> GetTagById(Guid tagId);
    Task<List<Tag>> GetAllTags();
    Task<List<Tag>> GetAudiotrackTags(Guid audiotrackId);
    Task<List<Audiotrack>> GetAudiotracksWithTags(List<Guid> tagId);
    Task AssignTagToAudiotrack(Guid audiotrackId, Guid tagId);
    Task RemoveTagFromAudiotrack(Guid audiotrackId, Guid tagId);

}