using Microsoft.EntityFrameworkCore;

using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class UserRepository(MewingPadDbContext context) : IUserRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(UserConverter.CoreToDbModel(user));
        await _context.Playlists.AddAsync(PlaylistConverter.CoreToDbModel(new(user.FavouritesId, "Favourites", user.Id)));
        await _context.SaveChangesAsync();
    }

    public Task<List<User>> GetAdmins()
    {
        return _context.Users
            .Where(u => u.IsAdmin == true)
            .Select(u => UserConverter.DbToCoreModel(u))
            .ToListAsync();
    }

    public Task<List<User>> GetAllUsers()
    {
        return _context.Users
            .Select(u => UserConverter.DbToCoreModel(u))
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return UserConverter.DbToCoreModel(user);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return UserConverter.DbToCoreModel(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        var userDbModel = await _context.Users.FindAsync(user.Id);

        userDbModel!.Id = user.Id;
        userDbModel!.FavouritesId = user.FavouritesId;
        userDbModel!.Username = user.Username;
        userDbModel!.PasswordHashed = user.PasswordHashed;
        userDbModel!.Email = user.Email;
        userDbModel!.IsAdmin = user.IsAdmin;

        await _context.SaveChangesAsync();
        return UserConverter.DbToCoreModel(userDbModel);
    }
}
