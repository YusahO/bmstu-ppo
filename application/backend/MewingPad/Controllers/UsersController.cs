using MewingPad.Services.PlaylistService;
using MewingPad.Services.UserService;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService,
                             IPlaylistService playlistService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IPlaylistService _playlistService = playlistService;
    private readonly Serilog.ILogger _logger = Log.ForContext<UsersController>();

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            _logger.Information("Retrieved user {@User}", new { user.Id, user.Username });
            return Ok(UserConverter.CoreModelToDto(user));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown: {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = from user in await _userService.GetAllUsers()
                        select UserConverter.CoreModelToDto(user);
            // log
            return Ok(users);
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{userId:guid}/playlists")]
    public async Task<IActionResult> GetUserPlaylists(Guid userId)
    {
        try
        {
            var playlists = await _playlistService.GetUserPlaylists(userId);
            return Ok(playlists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
