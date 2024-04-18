using System.Data.SqlClient;
using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class PlaylistRepository(MewingPadDbContext context) : IPlaylistRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<PlaylistRepository>();

    public async Task AddPlaylist(Playlist playlist)
    {
        _logger.Verbose("Entering AddPlaylist");

        try
        {
            await _context.Playlists.AddAsync(PlaylistConverter.CoreToDbModel(playlist));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddPlaylist");
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        _logger.Verbose("Entering DeletePlaylist");

        try
        {
            var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
            _context.Playlists.Remove(playlistDbModel!);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting DeletePlaylist");
    }

    public async Task<List<Playlist>> GetAllPlaylists()
    {
        _logger.Verbose("Entering GetAllPlaylists");

        List<Playlist> playlists;
        try
        {
            playlists = await _context.Playlists
                    .Select(p => PlaylistConverter.DbToCoreModel(p)!)
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllPlaylists");
        return playlists;
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        _logger.Verbose("Entering GetPlaylistById");

        Playlist? playlist;
        try
        {
            var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
            playlist = PlaylistConverter.DbToCoreModel(playlistDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetPlaylistById");
        return playlist;
    }

    public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
    {
        _logger.Verbose("Entering GetUserPlaylists");

        List<Playlist> playlists;

        try
        {
            playlists = await _context.Playlists
                   .Where(p => p.UserId == userId)
                   .Select(p => PlaylistConverter.DbToCoreModel(p))
                   .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetUserPlaylists");
        return playlists;
    }

    public async Task<Playlist> UpdatePlaylist(Playlist playlist)
    {
        _logger.Verbose("Entering UpdatePlaylist");

        try
        {
            var playlistDbModel = await _context.Playlists.FindAsync(playlist.Id);

            playlistDbModel!.Id = playlist.Id;
            playlistDbModel!.Title = playlist.Title;
            playlistDbModel!.UserId = playlist.UserId;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting UpdatePlaylist");
        return playlist;
    }
}