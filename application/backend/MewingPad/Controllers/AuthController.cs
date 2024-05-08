using MewingPad.Services.OAuthService;
using MewingPad.Common.Entities;
using MewingPad.UI.DTOs.Converters;
using MewingPad.UI.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using MewingPad.Services.UserService;
using Microsoft.AspNetCore.Authorization;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService,
                            IOAuthService oauthService,
                            IConfiguration configuration) : ControllerBase
{
    private readonly IUserService _userService = userService
                                                 ?? throw new ArgumentNullException(nameof(userService));
    private readonly IOAuthService _oauthService = oauthService
                                                   ?? throw new ArgumentNullException(nameof(oauthService));
    private readonly Serilog.ILogger _logger = Log.ForContext<AuthController>();
    private readonly IConfiguration _configuration = configuration;

    private void AddRefreshTokenCookie(string refreshToken)
    {
        _ = float.TryParse(_configuration["Jwt:RefreshTokenValidityInDays"], out float refreshTokenValidityInDays);
        var now = DateTime.UtcNow;
        var cookieOptions = new CookieOptions
        {
            Expires = now.AddDays(refreshTokenValidityInDays),
            // MaxAge = TimeSpan.FromDays(refreshTokenValidityInDays),
            HttpOnly = true,
        };
        Response.Cookies.Append(_configuration["CookieNames:RefreshToken"]!,
                                refreshToken,
                                cookieOptions);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Registration))]
    public async Task<IActionResult> Registration([FromBody] RegisterDto request)
    {
        try
        {
            var user = new User(Guid.NewGuid(),
                                Guid.NewGuid(),
                                request.Username!,
                                request.Email!,
                                request.Password!,
                                request.IsAdmin);

            var authData = await _oauthService.RegisterUser(user);
            AddRefreshTokenCookie(authData.TokensData!.RefreshToken!);
            
            var userDto = UserConverter.CoreModelToDto(user);
            var userAuthDto = new UserAuthDto
            {
                UserDto = userDto,
                TokenDto = new TokenDto { AccessToken = authData.TokensData!.AccessToken!,
                                          RefreshToken = authData.TokensData!.RefreshToken! }
            };
            return Ok(userAuthDto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var authData = await _oauthService.SignInUser(request.Email!, request.Password!);
            AddRefreshTokenCookie(authData.TokensData!.RefreshToken!);

            var userDto = UserConverter.CoreModelToDto(authData.User);
            var userAuthDto = new UserAuthDto
            {
                UserDto = userDto,
                TokenDto = new TokenDto { AccessToken = authData.TokensData!.AccessToken!,
                                          RefreshToken = authData.TokensData!.RefreshToken! }
            };
            return Ok(userAuthDto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception thrown");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}