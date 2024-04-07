using MewingPad.Common.Entities;

namespace MewingPad.Services.AudiotrackService;

public interface IAudiotrackService
{
    Task CreateAudiotrack(Audiotrack audiotrack);
    Task<Audiotrack> UpdateAudiotrack(Audiotrack audiotrack);
    Task DeleteAudiotrack(Guid audiotrackId);
    Task DownloadAudiotrack(string srcpath, string savepath);
    Task<Audiotrack> GetAudiotrackById(Guid audiotrackId);
    Task<List<Audiotrack>> GetAllAudiotracks();
    Task<List<Audiotrack>> GetAudiotracksByTitle(string title);
}
