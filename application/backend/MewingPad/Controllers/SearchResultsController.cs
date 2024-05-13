using MewingPad.Services.AudiotrackService;
using MewingPad.Services.TagService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MewingPad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchResultsController(IAudiotrackService audiotrackService,
                                         ITagService tagService) : ControllerBase
    {
        private readonly IAudiotrackService _audiotrackService = audiotrackService;
        private readonly ITagService _tagService = tagService;

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SearchAudiotracksByTags([FromQuery] List<Guid> tags)
        {
            try
            {
                if (tags.Count == 0)
                {
                    return Ok(new List<object>());
                }
                var audiotracks = await _tagService.GetAudiotracksWithTags(tags);
                return Ok(audiotracks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{title}")]
        public async Task<IActionResult> SearchAudiotracksByTitle(string title)
        {
            try
            {
                if (title.IsNullOrEmpty()) 
                {
                    return Ok(new List<object>());
                }
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