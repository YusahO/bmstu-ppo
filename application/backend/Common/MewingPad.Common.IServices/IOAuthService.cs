using MewingPad.Common.Entities;

namespace MewingPad.Services.OAuthService;

public interface IOAuthService
{
    Task<UserAuthData> RegisterUser(User user);
    Task<UserAuthData> SignInUser(string email, string password);
    // Task<string> RegenerateAccessToken(Guid userId);
}
