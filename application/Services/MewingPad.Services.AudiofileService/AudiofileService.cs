using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.AudiofileService;

public class AudiofileService(IAudiofileRepository audiofileRepository) : IAudiofileService
{
    private readonly IAudiofileRepository _audiofileRepository = audiofileRepository;

    public async Task CreateAudiofile(Audiofile audiofile)
    {
        if (await _audiofileRepository.GetAudiofileById(audiofile.Id) is not null)
        {
            throw new AudiofileExistsException(audiofile.Id);
        }
        await _audiofileRepository.AddAudiofile(audiofile);
    }

    public async Task<Audiofile> UpdateAudiofile(Audiofile audiofile)
    {
        if (await _audiofileRepository.GetAudiofileById(audiofile.Id) is null)
        {
            throw new AudiofileNotFoundException(audiofile.Id);
        }
        return await _audiofileRepository.UpdateAudiofile(audiofile);
    }

    public async Task DeleteAudiofile(Guid audiofileId)
    {
        if (await _audiofileRepository.GetAudiofileById(audiofileId) is null)
        {
            throw new AudiofileNotFoundException(audiofileId);
        }
        await _audiofileRepository.DeleteAudiofile(audiofileId);
    }
}