using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.PasswordHasher;
using Serilog;

namespace MewingPad.Services.OAuthService;

public class OAuthService(IUserRepository userRepository,
                          IPlaylistRepository playlistRepository) : IOAuthService
{
    private readonly IUserRepository _userRepository = userRepository
                                                       ?? throw new ArgumentNullException();
    private readonly IPlaylistRepository _playlistRepository = playlistRepository
                                                               ?? throw new ArgumentNullException();
    private readonly ILogger _logger = Log.ForContext<OAuthService>();

    public async Task RegisterUser(User user)
    {
        _logger.Verbose("Entering RegisterUser method");

        var foundUser = await _userRepository.GetUserByEmail(user.Email);
        if (foundUser is not null)
        {
            _logger.Error($"User with email \"{user.Email}\" already exists, cannot register");
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(user.PasswordHashed);
        await _userRepository.AddUser(user);
        await _playlistRepository.AddPlaylist(new(Guid.NewGuid(), "Favourites", user.Id));
        _logger.Verbose("Exiting RegisterUser method");
    }

    public async Task<User> SignInUser(string email, string password)
    {
        _logger.Verbose("Entering SignInUser method");
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