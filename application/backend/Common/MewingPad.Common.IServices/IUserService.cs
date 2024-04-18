using MewingPad.Common.Entities;
namespace MewingPad.Services.UserService;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUserById(Guid userId);
    Task<User> ChangeUserPermissions(Guid userId, bool isAdmin);
}