using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface IAudiofileRepository
{
    Task AddAudiofile(Audiofile audiofile);
    Task<Audiofile> UpdateAudiofile(Audiofile audiofile);
    Task DeleteAudiofile(Guid id);
    Task<Audiofile?> GetAudiofileById(Guid id);
    Task<List<Audiofile>> GetAllAudiofiles();
}
