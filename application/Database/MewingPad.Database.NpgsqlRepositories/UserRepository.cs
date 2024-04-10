using Microsoft.EntityFrameworkCore;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class UserRepository(MewingPadDbContext context) : IUserRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<UserRepository>();

    public async Task AddUser(User user)
    {
        _logger.Verbose("Entering AddUser method");

        try
        {
            await _context.Users.AddAsync(UserConverter.CoreToDbModel(user));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting AddUser method");
    }

    public async Task<List<User>> GetAdmins()
    {
        _logger.Verbose("Entering GetAdmins method");
        
        var admins = await _context.Users
            .Where(u => u.IsAdmin == true)
            .Select(u => UserConverter.DbToCoreModel(u))
            .ToListAsync();
        if (admins.Count == 0)
        {
            _logger.Warning("No admins found in database");
        }

        _logger.Verbose("Exiting GetAdmins method");
        return admins;
    }

    public async Task<List<User>> GetAllUsers()
    {
        _logger.Verbose("Entering GetAllUsers method");

        var users = await _context.Users
            .Select(u => UserConverter.DbToCoreModel(u))
            .ToListAsync();
        if (users.Count == 0)
        {
            _logger.Warning("Database has no entries of User");
        }

        _logger.Verbose("Exiting GetAllUsers method");
        return users;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        _logger.Verbose("Entering GetUserByEmail method");

        var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userDbModel is null)
        {
            _logger.Warning($"User (Email = {email}) not found in database");
        }
        var user = UserConverter.DbToCoreModel(userDbModel);

        _logger.Verbose("Exiting GetUserByEmail method");
        return user;
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        _logger.Verbose("Entering GetUserById method");

        var userDbModel = await _context.Users.FindAsync(userId);
        if (userDbModel is null)
        {
            _logger.Warning($"User (Id = {userId}) not found in database");
        }
        var user = UserConverter.DbToCoreModel(userDbModel);

        _logger.Verbose("Exiting GetUserById method");
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _logger.Verbose("Entering UpdateUser method");

        var userDbModel = await _context.Users.FindAsync(user.Id);

        userDbModel!.Id = user.Id;
        userDbModel!.FavouritesId = user.FavouritesId;
        userDbModel!.Username = user.Username;
        userDbModel!.PasswordHashed = user.PasswordHashed;
        userDbModel!.Email = user.Email;
        userDbModel!.IsAdmin = user.IsAdmin;

        await _context.SaveChangesAsync();
        _logger.Information($"Updated User (Id = {user.Id})");
        _logger.Verbose("Exiting UpdateUser method");
        return user;
    }
}
