using MewingPad.Services.AudiotrackService;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudiotracksController(IAudiotrackService audiotrackService) : ControllerBase
{
    private readonly IAudiotrackService _audiotrackService = audiotrackService
                                                             ?? throw new ArgumentNullException(nameof(audiotrackService));
    private readonly Serilog.ILogger _logger = Log.ForContext<AudiotracksController>();
    
    [HttpGet]
    public async Task<IActionResult> GetAllAudiotracks()
    {
        try
        {
            var audiotracks = from audiotrack in await _audiotrackService.GetAllAudiotracks()
                              select AudiotrackConverter.CoreModelToDto(audiotrack);
            // log
            return Ok(audiotracks);
        }
        catch (Exception ex)
        {   
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{filename}")]
    public async Task<IActionResult> GetAudiotrackFile(string filename)
    {
        try
        {
            var audioStream = await _audiotrackService.GetAudiotrackFileStream(filename);
            // log
            return File(audioStream, "application/octet-stream");
        }
        catch (Exception ex)
        {   
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}