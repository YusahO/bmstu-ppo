using MewingPad.Services.AudiotrackService;
using MewingPad.Services.TagService;
using MewingPad.UI.DTOs;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudiotracksController(IAudiotrackService audiotrackService,
                                   ITagService tagService) : ControllerBase
{
    private readonly IAudiotrackService _audiotrackService = audiotrackService;
    private readonly ITagService _tagService = tagService;

    private readonly Serilog.ILogger _logger = Log.ForContext<AudiotracksController>();

    [AllowAnonymous]
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAudiotrack([FromForm] AudiotrackFormDto audiotrackDto)
    {
        try
        {
            _logger.Information("Audiotrack dto {@Audio} received", audiotrackDto);
            if (audiotrackDto.File is null || audiotrackDto.File.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }
            using var memoryStream = new MemoryStream();
            await audiotrackDto.File.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var audiotrack = AudiotrackFormConverter.DtoToCoreModel(audiotrackDto);
            audiotrack.Id = Guid.NewGuid();
            await _audiotrackService.CreateAudiotrackWithStream(memoryStream, audiotrack);
            _logger.Information("Audiotrack {@Audio} uploaded successfully", audiotrack);

            return Ok("Audiotrack uploaded successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteAudiotrack([FromBody] AudiotrackDto audiotrackDto)
    {
        try
        {
            var audiotrack = AudiotrackConverter.DtoToCoreModel(audiotrackDto);
            await _audiotrackService.DeleteAudiotrack(audiotrack.Id);
            return Ok("Audiotrack deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAudiotrack([FromForm] AudiotrackFormDto audiotrackDto)
    {
        try
        {
            _logger.Information("Audiotrack dto {@Audio} received", audiotrackDto);
            if (audiotrackDto.File is null || audiotrackDto.File.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }
            using var memoryStream = new MemoryStream();
            await audiotrackDto.File.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var audiotrack = AudiotrackFormConverter.DtoToCoreModel(audiotrackDto);
            await _audiotrackService.UpdateAudiotrackWithStream(memoryStream, audiotrack);
            _logger.Information("Audiotrack {@Audio} updated successfully", audiotrack);

            return Ok("Audiotrack updated successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("{audiotrackId:guid}/tags")]
    public async Task<IActionResult> GetAudiotrackTags(Guid audiotrackId)
    {
        try
        {
            var tags = from tag in await _tagService.GetAudiotrackTags(audiotrackId)
                       select TagConverter.CoreModelToDto(tag);
            // log
            return Ok(tags);
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{audiotrackId:guid}/tags")]
    public async Task<IActionResult> AddTagToAudiotrack(Guid audiotrackId, [FromBody] Guid tagId)
    {
        try
        {
            await _tagService.AssignTagToAudiotrack(audiotrackId, tagId);
            return Ok("Tag added successfully");
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{audiotrackId:guid}/tags")]
    public async Task<IActionResult> DeleteTagFromAudiotrack(Guid audiotrackId, [FromBody] Guid tagId)
    {
        try
        {
            await _tagService.DeleteTagFromAudiotrack(audiotrackId, tagId);
            return Ok("Tag removed successfully");
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}