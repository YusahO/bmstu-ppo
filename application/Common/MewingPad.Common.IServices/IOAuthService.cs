using MewingPad.Common.Entities;

namespace MewingPad.Services.OAuthService;

public interface IOAuthService
{
    Task RegisterUser(User user);
    Task<User> SignInUser(string email, string password);
    Task SignOutUser(User user);
}
