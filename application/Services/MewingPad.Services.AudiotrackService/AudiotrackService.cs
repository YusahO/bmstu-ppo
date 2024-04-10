using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.AudioManager;
using Serilog;

namespace MewingPad.Services.AudiotrackService;

public class AudiotrackService(IAudiotrackRepository audiotrackRepository,
                               IPlaylistAudiotrackRepository playlistAudiotrackRepository,
                               ITagAudiotrackRepository tagAudiotrackRepository) : IAudiotrackService
{
    private readonly IAudiotrackRepository _audiotrackRepository = audiotrackRepository;
    private readonly IPlaylistAudiotrackRepository _playlistAudiotrackRepository = playlistAudiotrackRepository;
    private readonly ITagAudiotrackRepository _tagAudiotrackRepository = tagAudiotrackRepository;
    private readonly ILogger _logger = Log.ForContext<AudiotrackService>();

    public async Task CreateAudiotrack(Audiotrack audiotrack)
    {
        _logger.Information("Entering CreateAudiotrack method");

        if (await _audiotrackRepository.GetAudiotrackById(audiotrack.Id) is not null)
        {
            _logger.Error("Audiotrack {@Audio} already exists",
                          new { audiotrack.Id, audiotrack.Title });
            throw new AudiotrackExistsException(audiotrack.Id);
        }

        await _audiotrackRepository.AddAudiotrack(audiotrack);
        _logger.Information("Created audiotrack {@Audio} in database",
                            new { audiotrack.Id, audiotrack.Title });

        if (!await AudioManager.CreateFileAsync(audiotrack.Filepath))
        {
            _logger.Error("Failed to upload audiotrack with path \"{audiotrack.Filepath}\"");
            throw new AudiotrackServerUploadException("Failed to upload audiotrack with path \"{audiotrack.Filepath}\"");
        }
        _logger.Information("Exiting CreateAudiotrack method");
    }

    public async Task<Audiotrack> UpdateAudiotrack(Audiotrack audiotrack)
    {
        _logger.Information("Entering UpdateAudiotrack method");

        var oldAudiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrack.Id);
        if (oldAudiotrack is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrack.Id}) not found");
            throw new AudiotrackNotFoundException(audiotrack.Id);
        }

        if (oldAudiotrack.Filepath != audiotrack.Filepath)
        {
            if (!await AudioManager.UpdateFileAsync(oldAudiotrack.Filepath, audiotrack.Filepath))
            {
                _logger.Error(
                    $"Failed to update audiotrack (path = \"{oldAudiotrack.Filepath}\")" +
                    $" with path \"{audiotrack.Filepath}\"");
                throw new AudiotrackServerUpdateException(
                    $"Failed to update audiotrack (path = \"{oldAudiotrack.Filepath}\")" +
                    $" with path \"{audiotrack.Filepath}\"");
            }
        }

        await _audiotrackRepository.UpdateAudiotrack(audiotrack);
        _logger.Information("Exiting UpdateAudiotrack method");
        return audiotrack;
    }

    public async Task DeleteAudiotrack(Guid audiotrackId)
    {
        _logger.Information($"Entering DeleteAudiotrack method");

        var audiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrackId);
        if (audiotrack is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }

        if (!await AudioManager.DeleteFileAsync(audiotrack.Filepath))
        {
            _logger.Error($"Failed to delete audiotrack with path \"{audiotrack.Filepath}\"");
            throw new AudiotrackServerDeleteException($"Failed to delete audiotrack with path \"{audiotrack.Filepath}\"");
        }

        await _tagAudiotrackRepository.DeleteByAudiotrack(audiotrackId);
        await _playlistAudiotrackRepository.DeleteByAudiotrack(audiotrackId);
        await _audiotrackRepository.DeleteAudiotrack(audiotrackId);

        _logger.Information("Exiting DeleteAudiotrack method");
    }

    public async Task<Audiotrack> GetAudiotrackById(Guid audiotrackId)
    {
        _logger.Information("Entering GetAudiotrackById method");

        var audiotrack = await _audiotrackRepository.GetAudiotrackById(audiotrackId);
        if (audiotrack is null)
        {
            _logger.Error($"Audiotrack (Id = {audiotrackId}) not found");
            throw new AudiotrackNotFoundException(audiotrackId);
        }

        _logger.Information("Exiting GetAudiotrackById method");
        return audiotrack;
    }

    public Task<List<Audiotrack>> GetAllAudiotracks()
    {
        _logger.Information("Entering GetAllAudiotracks method");
        var audios = _audiotrackRepository.GetAllAudiotracks();
        _logger.Information("Exiting GetAllAudiotracks method");
        return audios;
    }

    public async Task<List<Audiotrack>> GetAudiotracksByTitle(string title)
    {
        _logger.Information("Entering GetAudiotracksByTitle method");
        var audios = await _audiotrackRepository.GetAudiotracksByTitle(title);
        _logger.Information("Exiting GetAudiotracksByTitle method");
        return audios;
    }

    public async Task DownloadAudiotrack(string srcpath, string savepath)
    {
        _logger.Information("Entering DownloadAudiotrack method");
        if (!await AudioManager.GetFileAsync(srcpath, savepath))
        {
            _logger.Error($"Failed to get audiotrack (\"{srcpath}\") from server");
            throw new AudiotrackServerGetException($"{srcpath} does not exist");
        }
        _logger.Information("Exiting DownloadAudiotrack method");
    }
}