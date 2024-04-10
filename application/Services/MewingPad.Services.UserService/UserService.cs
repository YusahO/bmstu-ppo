using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using Serilog;

namespace MewingPad.Services.UserService;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _userRepository = repository
                                                       ?? throw new ArgumentNullException();

    private readonly ILogger _logger = Log.ForContext<UserService>();

    public async Task<User> ChangeUserPermissions(Guid userId, bool isAdmin)
    {
        _logger.Verbose("Entering ChangeUserPermissions method");

        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            _logger.Error($"User (Id = {userId}) not found");
            throw new UserNotFoundException(userId);
        }
        user.IsAdmin = isAdmin;
        await _userRepository.UpdateUser(user);

        _logger.Verbose("Exiting ChangeUserPermissions method");
        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        _logger.Verbose("Entering GetAllUsers method");

        var users = await _userRepository.GetAllUsers();

        _logger.Verbose("Exiting GetAllUsers method");
        return users;
    }

    public async Task<User> GetUserById(Guid userId)
    {
        _logger.Verbose("Entering GetUserById method");

        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            _logger.Error($"User (Id = {userId}) not found");
            throw new UserNotFoundException(userId);
        }

        _logger.Verbose("Exiting GetUserById method");
        return user;
    }
}
