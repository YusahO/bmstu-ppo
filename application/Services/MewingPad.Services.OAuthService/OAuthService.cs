using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Utils.PasswordHasher;

namespace MewingPad.Services.OAuthService;

public class OAuthService(IUserRepository repository) : IOAuthService
{
    private readonly IUserRepository _userRepository = repository
                                                       ?? throw new ArgumentNullException();

    public async Task RegisterUser(User user, string password)
    {
        User foundUser = await _userRepository.GetUserByEmail(user.Email);
        if (foundUser is not null)
        {
            throw new UserRegisteredException($"User with email \"{user.Email}\" already registered");
        }
        user.PasswordHashed = PasswordHasher.HashPassword(password);
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
        user.IsAuthorized = true;
        await _userRepository.UpdateUser(user);
        return user;
    }

    public async Task SignOutUser(User user)
    {
        User foundUser = await _userRepository.GetUserById(user.Id)
                         ?? throw new UserNotFoundException(user.Id);
        foundUser.IsAuthorized = false;
        await _userRepository.UpdateUser(user);
    }
}