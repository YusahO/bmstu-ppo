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


    public async Task<User> RegisterUser(User user)
    {
        _logger.Verbose("Entering RegisterUser");

        var foundUser = await _userRepository.GetUserByEmail(user.Email);
        if (foundUser is not null)
        {
            _logger.Error($"User with email \"{user.Email}\" already exists, cannot register");
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(user.PasswordHashed);

        await _userRepository.AddUser(user);
        _logger.Information($"User (Id = {user.Id}) added");

        var favourites = new Playlist(user.FavouritesId, _favouritesName, user.Id);
        await _playlistRepository.AddPlaylist(favourites);
        _logger.Information($"User favourites playlist (Id = {favourites.Id}) added");

        _logger.Verbose("Exiting RegisterUser");
        return user;
    }

    public async Task SaveRefreshToken(Guid userId, string refreshToken)
    {
        await _userRepository.SaveRefreshToken(userId, refreshToken);
    }

    public async Task<User> SignInUser(string email, string password)
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

        _logger.Verbose("Exiting SignInUser method");
        return user;
    }
}