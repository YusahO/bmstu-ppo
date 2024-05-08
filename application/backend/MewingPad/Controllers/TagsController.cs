using MewingPad.Services.TagService;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController(ITagService tagService) : ControllerBase
    {
        private readonly ITagService _tagService = tagService;
        private readonly Serilog.ILogger _logger = Log.ForContext<TagsController>();

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tags = from tag in await _tagService.GetAllTags()
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

        [AllowAnonymous]
        [HttpGet("{audiotrackId:guid}")]
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
    }
}