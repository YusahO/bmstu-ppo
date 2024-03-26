using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
namespace MewingPad.Services.UserService;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _userRepository = repository
                                                       ?? throw new ArgumentNullException();

    public async Task<User> ChangeUserPermissions(Guid userId, bool isAdmin)
    {
        var user = await _userRepository.GetUserById(userId)
                   ?? throw new UserNotFoundException($"User ID = {userId} not found");
        user.IsAdmin = isAdmin;
        return await _userRepository.UpdateUser(user);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<User> GetUserById(Guid userId)
    {
        User foundUser = await _userRepository.GetUserById(userId)
                         ?? throw new UserNotFoundException($"User ID = {userId} not found");
        return foundUser;
    }
}
