using MewingPad.Common.Entities;
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
        _logger.Information("Entering AddPlaylist method");

        try
        {
            await _context.Playlists.AddAsync(PlaylistConverter.CoreToDbModel(playlist));
            await _context.SaveChangesAsync();
            _logger.Information($"Added playlist (Id = {playlist.Id}) to database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting AddPlaylist method");
    }

    public async Task DeletePlaylist(Guid playlistId)
    {
        _logger.Information("Entering DeletePlaylist method");

        try
        {
            var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
            _context.Playlists.Remove(playlistDbModel!);
            await _context.SaveChangesAsync();
            _logger.Information($"Deleted playlist (Id = {playlistId}) from database");
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Information("Exiting DeletePlaylist method");
    }

    public async Task<List<Playlist>> GetAllPlaylists()
    {
        _logger.Information("Entering GetAllPlaylists method");

        var playlists = await _context.Playlists
            .Select(p => PlaylistConverter.DbToCoreModel(p)!)
            .ToListAsync();
        if (playlists.Count == 0)
        {
            _logger.Warning("Database contains no records of Playlist");
        }
        
        _logger.Information("Exiting GetAllPlaylists method");
        return playlists;
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        _logger.Information("Entering GetPlaylistById method");

        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        if (playlistDbModel is null)
        {
            _logger.Warning($"Playlist (Id = {playlistId}) not found in database");
        }
        var playlist = PlaylistConverter.DbToCoreModel(playlistDbModel);

        _logger.Information("Exiting GetPlaylistById method");
        return playlist;
    }

    public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
    {
        _logger.Information("Entering GetUserPlaylists method");

        var playlists = await _context.Playlists
            .Where(p => p.UserId == userId)
            .Select(p => PlaylistConverter.DbToCoreModel(p))
            .ToListAsync();
        if (playlists.Count == 0)
        {
            _logger.Warning($"User (Id = {userId}) has no playlists");
        }
        
        _logger.Information("Exiting GetUserPlaylists method");
        return playlists;
    }

    public async Task<Playlist> UpdatePlaylist(Playlist playlist)
    {
        _logger.Information("Entering UpdatePlaylist method");

        var playlistDbModel = await _context.Playlists.FindAsync(playlist.Id);

        playlistDbModel!.Id = playlist.Id;
        playlistDbModel!.Title = playlist.Title;
        playlistDbModel!.UserId = playlist.UserId;

        await _context.SaveChangesAsync();
        _logger.Information($"Playlist (Id = {playlist.Id}) updated");
        _logger.Information("Exiting UpdatePlaylist method");
        return playlist;
    }
}