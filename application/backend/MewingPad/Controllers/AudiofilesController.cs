using MewingPad.Services.AudiotrackService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudiofilesController(IAudiotrackService audiotrackService) : ControllerBase
{
    private readonly IAudiotrackService _audiotrackService = audiotrackService;

    [AllowAnonymous]
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