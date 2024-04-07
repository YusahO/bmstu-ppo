using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.AudioManager;

namespace MewingPad.Services.AudiotrackService;

public class AudiotrackService(IAudiotrackRepository audiotrackRepository) : IAudiotrackService
{
    private readonly IAudiotrackRepository _audiotrackRepository = audiotrackRepository;

    public async Task CreateAudiotrack(Audiotrack audiotrack)
    {
        if (await _audiotrackRepository.GetAudiotrackById(audiotrack.Id) is not null)
        {
            throw new AudiotrackExistsException(audiotrack.Id);
        }
        await _audiotrackRepository.AddAudiotrack(audiotrack);
        await AudioManager.CreateFileAsync(audiotrack.Filepath);
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiotrack)
    {
        var oldAudiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrack.Id)
                            ?? throw new AudiotrackNotFoundException(audiotrack.Id);
        if (oldAudiotrack.Filepath != audiotrack.Filepath)
        {
            await AudioManager.UpdateFileAsync(oldAudiotrack.Filepath, audiotrack.Filepath);
        }
        return await _audiotrackRepository.UpdateAudiotrack(audiotrack);
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        var audiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrackId)
                         ?? throw new AudiotrackNotFoundException(audiotrackId);
        await AudioManager.DeleteFileAsync(audiotrack.Filepath);
        await _audiotrackRepository.DeleteAudiotrack(audiotrackId);
    }

    public async Task<Audiotrack> GetAudiotrackById(Guid audiotrackId)
    {
        var audiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrackId)
                              ?? throw new AudiotrackNotFoundException(audiotrackId);
        return audiotrack;
    }

    public Task<List<Audiotrack>> GetAllAudiotracks()
    {
        return _audiotrackRepository.GetAllAudiotracks();
    }

    public Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        return _audiotrackRepository.GetAudiotracksByTitle(title);
    }

    public async Task DownloadAudiotrack(string srcpath, string savepath)
    {
        if (!await AudioManager.GetFileAsync(srcpath, savepath))
        {
            throw new Exception($"{srcpath} does not exist");
        }
    }
}