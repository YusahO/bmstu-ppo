﻿using Microsoft.EntityFrameworkCore;
using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Serilog;
using MewingPad.Common.Exceptions;

namespace MewingPad.Database.NpgsqlRepositories;

public class UserRepository(MewingPadDbContext context) : IUserRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<UserRepository>();

    public async Task AddUser(User user)
    {
        _logger.Verbose("Entering AddUser");

        try
        {
            await _context.Users.AddAsync(UserConverter.CoreToDbModel(user));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddUser");
    }

    public async Task<List<User>> GetAdmins()
    {
        _logger.Verbose("Entering GetAdmins");

        List<User> admins;
        try
        {
            admins = await _context.Users
                    .Where(u => u.IsAdmin == true)
                    .Select(u => UserConverter.DbToCoreModel(u))
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAdmins");
        return admins;
    }

    public async Task<List<User>> GetAllUsers()
    {
        _logger.Verbose("Entering GetAllUsers");

        List<User> users;
        try
        {
            users = await _context.Users
                    .Select(u => UserConverter.DbToCoreModel(u))
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllUsers");
        return users;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        _logger.Verbose("Entering GetUserByEmail");

        User? user;
        try
        {
            var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            user = UserConverter.DbToCoreModel(userDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetUserByEmail");
        return user;
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        _logger.Verbose("Entering GetUserById");

        User? user;
        try
        {
            var userDbModel = await _context.Users.FindAsync(userId);
            user = UserConverter.DbToCoreModel(userDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetUserById");
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _logger.Verbose("Entering UpdateUser");

        try
        {
            var userDbModel = await _context.Users.FindAsync(user.Id);

            userDbModel!.Id = user.Id;
            userDbModel!.FavouritesId = user.FavouritesId;
            userDbModel!.Username = user.Username;
            userDbModel!.PasswordHashed = user.PasswordHashed;
            userDbModel!.Email = user.Email;
            userDbModel!.IsAdmin = user.IsAdmin;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting UpdateUser");
        return user;
    }
}
