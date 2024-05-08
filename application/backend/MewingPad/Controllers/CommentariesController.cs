using MewingPad.Services.CommentaryService;
using MewingPad.Services.UserService;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentariesController(ICommentaryService commentaryService,
                                    IUserService userService) : ControllerBase
{
    private readonly ICommentaryService _commentaryService = commentaryService;
    private readonly IUserService _userService = userService;

    [AllowAnonymous]
    [HttpGet("{audiotrackId:guid}")]
    public async Task<IActionResult> GetAudiotrackCommentaries(Guid audiotrackId)
    {
        try
        {
            var comms = (from comm in await _commentaryService.GetAudiotrackCommentaries(audiotrackId)
                        select CommentaryConverter.CoreModelToDto(comm)).ToList();

            for (int i = 0; i < comms.Count; ++i)
            {
                var username = (await _userService.GetUserById(comms[i].AuthorId)).Username;
                comms[i].AuthorName = username;
            }

            return Ok(comms);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}