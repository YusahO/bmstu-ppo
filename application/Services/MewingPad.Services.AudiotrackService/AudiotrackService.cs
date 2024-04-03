using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.AudiotrackService;

public class AudiotrackService(IAudiotrackRepository audiotrackRepository) : IAudiotrackService
{
    private readonly IAudiotrackRepository _audiofileRepository = audiotrackRepository;

    public async Task CreateAudiotrack(Audiotrack audiofile)
    {
        if (await _audiofileRepository.GetAudiotrackById(audiofile.Id) is not null)
        {
            throw new AudiotrackExistsException(audiofile.Id);
        }
        await _audiofileRepository.AddAudiotrack(audiofile);
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiofile)
    {
        if (await _audiofileRepository.GetAudiotrackById(audiofile.Id) is null)
        {
            throw new AudiotrackNotFoundException(audiofile.Id);
        }
        return await _audiofileRepository.UpdateAudiotrack(audiofile);
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        if (await _audiofileRepository.GetAudiotrackById(audiotrackId) is null)
        {
            throw new AudiotrackNotFoundException(audiotrackId);
        }
        await _audiofileRepository.DeleteAudiotrack(audiotrackId);
    }

    public async Task<Audiotrack> GetAudiotrackById(Guid audiotrackId)
    {
        var audiofile = await _audiofileRepository.GetAudiotrackById(audiotrackId)
                              ?? throw new AudiotrackNotFoundException(audiotrackId);
        return audiofile;
    }

    public Task<List<Audiotrack>> GetAllAudiotracks()
    {
        return _audiofileRepository.GetAllAudiotracks();
    }

    public Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        return _audiofileRepository.GetAudiotracksByTitle(title);
    }
}