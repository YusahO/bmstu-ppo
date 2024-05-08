using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.PasswordHasher;
using MewingPad.Utils.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MewingPad.Services.OAuthService;

public class OAuthService(IConfiguration config,
                          IUserRepository userRepository,
                          IPlaylistRepository playlistRepository) : IOAuthService
{
    private readonly IUserRepository _userRepository = userRepository
                                                       ?? throw new ArgumentNullException();
    private readonly IPlaylistRepository _playlistRepository = playlistRepository
                                                               ?? throw new ArgumentNullException();
    private readonly ILogger _logger = Log.ForContext<OAuthService>();
    private readonly IConfiguration _config = config;
    private readonly string _favouritesName = config["ApiSettings:FavouritesDefaultName"]!;

    // private string GenerateAccessToken(User user)
    // {
    //     _logger.Verbose("Entering GenerateAccessToken({@User})", user);
    //     var now = DateTime.UtcNow;

    //     _ = float.TryParse(_config["Jwt:AccessTokenValidityInMinutes"], out float tokenValidityInMinutes);
    //     var expires = now.Add(TimeSpan.FromMinutes(tokenValidityInMinutes));

    //     var claims = new List<Claim>{
    //         new(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //         new(ClaimTypes.Email, user.Email),
    //         new(ClaimTypes.Name, user.Username)
    //     };

    //     var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Secret"]!));

    //     var jwt = new JwtSecurityToken(
    //         issuer: _config["Jwt:Issuer"],
    //         audience: _config["Jwt:Audience"],
    //         claims: claims,
    //         expires: expires,
    //         signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
    //     );

    //     _logger.Verbose("Exiting GenerateAccessToken", user);
    //     return new JwtSecurityTokenHandler().WriteToken(jwt);
    // }

    // private string GenerateRefreshToken()
    // {
    //     _logger.Verbose("Entering GenerateRefreshToken");

    //     var randomNumber = new byte[32];

    //     using var rng = RandomNumberGenerator.Create();
    //     rng.GetBytes(randomNumber);

    //     _logger.Verbose("Exiting GenerateRefreshToken");
    //     return Convert.ToBase64String(randomNumber);
    // }

    // public TokensData GenerateTokens(User user)
    // {
    //     // return new TokensData
    //     // {
    //     //     AccessToken = GenerateAccessToken(user),
    //     //     RefreshToken = GenerateRefreshToken(),
    //     // };
    //     return default( TokensData )!;
    // }

    public async Task<UserAuthData> RegisterUser(User user)
    {
        _logger.Verbose("Entering RegisterUser");

        var foundUser = await _userRepository.GetUserByEmail(user.Email);
        if (foundUser is not null)
        {
            _logger.Error($"User with email \"{user.Email}\" already exists, cannot register");
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(user.PasswordHashed);

        var tokens = new TokensData
        {
            AccessToken = TokenUtils.GenerateAccessToken(user.Id.ToString(), user.Email, user.Username),
            RefreshToken = TokenUtils.GenerateRefreshToken()
        };


        await _userRepository.AddUser(user);
        _logger.Information($"User (Id = {user.Id}) added");

        await _userRepository.SaveRefreshToken(user.Id, tokens.RefreshToken!);
        _logger.Debug($"User (Id = {user.Id}) refresh token saved");

        var favourites = new Playlist(Guid.NewGuid(), _favouritesName, user.Id);
        await _playlistRepository.AddPlaylist(favourites);
        _logger.Information($"User favourites playlist (Id = {favourites.Id}) added");

        _logger.Verbose("Exiting RegisterUser");
        return new UserAuthData { User = user, TokensData = tokens };
    }

    public async Task<UserAuthData> SignInUser(string email, string password)
    {
        _logger.Verbose($"Entering SignInUser({email})");
        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
        {
            _logger.Error($"User with email \"{email}\" not found");
            throw new UserNotFoundException($"User with email \"{email}\" not found");
        }

        if (!PasswordHasher.VerifyPassword(password, user.PasswordHashed))
        {
            _logger.Error($"Incorrect password for user \"{email}\"");
            throw new UserCredentialsException($"Incorrect password for user with login \"{email}\"");
        }

        var tokens = new TokensData
        {
            AccessToken = TokenUtils.GenerateAccessToken(user.Id.ToString(), user.Email, user.Username),
            RefreshToken = TokenUtils.GenerateRefreshToken()
        };

        await _userRepository.SaveRefreshToken(user.Id, tokens.RefreshToken!);
        _logger.Debug($"User (Id = {user.Id}) refresh token saved");

        _logger.Verbose("Exiting SignInUser method");
        return new UserAuthData { User = user, TokensData = tokens };
    }

    // public async Task<string> RegenerateAccessToken(Guid userId)
    // {
    //     if ((await _userRepository.GetUserById(userId)) is null)
    //     {
    //         _logger.Error($"User with id \"{userId}\" not found");
    //         throw new UserNotFoundException(userId);
    //     }

    //     string refreshToken = await _userRepository.GetUserRefreshToken(userId);
    // }
}