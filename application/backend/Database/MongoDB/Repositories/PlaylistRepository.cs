using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.MongoDB.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.MongoDB.Repositories;

public class PlaylistRepository(MewingPadMongoDbContext context) : IPlaylistRepository
{
    private readonly MewingPadMongoDbContext _context = context;
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
            var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId);
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
            var found = await _context.Playlists.ToListAsync();
            playlists = found.Select(p => PlaylistConverter.DbToCoreModel(p)).ToList();
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
            var found = await _context.Playlists
                .Where(p => p.UserId == userId)
                .ToListAsync();
            playlists = found.Select(p => PlaylistConverter.DbToCoreModel(p)).ToList();
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