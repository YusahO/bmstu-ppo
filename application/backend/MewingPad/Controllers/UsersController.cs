using MewingPad.Services.UserService;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    private readonly Serilog.ILogger _logger = Log.ForContext<UsersController>();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            // _logger.Information("Retrieved user {@User}", new { user.Id, user.Username });
            Log.Information("Retrieved user {@User}", new { user.Id, user.Username });
            return Ok(UserConverter.CoreModelToDto(user));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown: {Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

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
}