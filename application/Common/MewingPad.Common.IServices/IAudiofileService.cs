using MewingPad.Common.Entities;

namespace MewingPad.Services.AudiofileService;

public interface IAudiofileService
{
    Task CreateAudiofile(Audiofile audiofile);
    Task<Audiofile> UpdateAudiofile(Audiofile audiofile);
    Task DeleteAudiofile(Guid audiofileId);
    Task<Audiofile> GetAudiofileById(Guid audiofileId);
}
