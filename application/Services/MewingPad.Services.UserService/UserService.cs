using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
namespace MewingPad.Services.UserService;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _userRepository = repository
                                                       ?? throw new ArgumentNullException();

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<User> GetUserById(Guid id)
    {
        User foundUser = await _userRepository.GetUserById(id)
                         ?? throw new UserNotFoundException($"User ID = {id} not found");
        return foundUser;
    }
}
