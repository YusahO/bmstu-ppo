using MewingPad.Common.Entities;

namespace MewingPad.Common.IRepositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task SaveRefreshToken(Guid userId, string refreshToken);
    Task<string> GetUserRefreshToken(Guid userId);
    Task<User> UpdateUser(User user);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task<List<User>> GetAdmins();
}
