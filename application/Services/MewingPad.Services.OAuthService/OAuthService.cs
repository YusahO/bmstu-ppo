using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.PasswordHasher;

namespace MewingPad.Services.OAuthService;

public class OAuthService(IUserRepository repository) : IOAuthService
{
    private readonly IUserRepository _userRepository = repository
                                                       ?? throw new ArgumentNullException();

    public async Task RegisterUser(User user)
    {
        var foundUser = await _userRepository.GetUserByEmail(user.Email);
        if (foundUser is not null)
        {
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(user.PasswordHashed);
        await _userRepository.AddUser(user);
    }

    public async Task<User> SignInUser(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email)
                    ?? throw new UserNotFoundException($"User with email \"{email}\" not found");
        if (!PasswordHasher.VerifyPassword(password, user.PasswordHashed))
        {
            throw new UserCredentialsException($"Incorrect password for user with login \"{email}\"");
        }
        await _userRepository.UpdateUser(user);
        return user;
    }
}