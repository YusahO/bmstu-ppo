using MewingPad.Services.AudiotrackService;
using MewingPad.Services.TagService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(IAudiotrackService audiotrackService,
                                  ITagService tagService) : ControllerBase
    {
        private readonly IAudiotrackService _audiotrackService = audiotrackService;
        private readonly ITagService _tagService = tagService;

        [AllowAnonymous]
        [HttpGet("{title}")]
        public async Task<IActionResult> SearchAudiotracksByTitle(string title)
        {
            try
            {
                var audiotracks = await _audiotrackService.GetAudiotracksByTitle(title);
                return Ok(audiotracks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}