using MewingPad.Services.CommentaryService;
using MewingPad.Services.UserService;
using MewingPad.UI.DTOs;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentariesController(ICommentaryService commentaryService,
                                    IUserService userService) : ControllerBase
{
    private readonly ICommentaryService _commentaryService = commentaryService;
    private readonly IUserService _userService = userService;
    private readonly Serilog.ILogger _logger = Log.ForContext<CommentariesController>();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddCommentary([FromBody] CommentaryDto commentary)
    {
        try
        {
            var comm = CommentaryConverter.DtoToCoreModel(commentary);
            comm.Id = Guid.NewGuid();
            await _commentaryService.CreateCommentary(comm);
            
            _logger.Information("Add commentary {@Comm}", comm);
            return Ok("Commentary added successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateCommentary([FromBody] CommentaryDto commentary)
    {
        try
        {
            var comm = CommentaryConverter.DtoToCoreModel(commentary);
            await _commentaryService.UpdateCommentary(comm);

            _logger.Information("Updated commentary {@Comm}", comm);
            return Ok("Commentary updated successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteCommentary([FromBody] CommentaryDto commentary)
    {
        try
        {
            var comm = CommentaryConverter.DtoToCoreModel(commentary);
            await _commentaryService.DeleteCommentary(comm.Id);
            
            _logger.Information("Deleted commentary {@Comm}", comm);
            return Ok("Commentary deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}