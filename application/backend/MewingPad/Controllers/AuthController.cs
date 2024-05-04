using System.Security.Claims;
using MewingPad.Services.OAuthService;
using MewingPad.UI.DTOs.Auth;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MewingPad.Services.UserService;
using System.Security.Cryptography;

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

    
}