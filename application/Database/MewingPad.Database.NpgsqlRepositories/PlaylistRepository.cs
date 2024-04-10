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
        return await _context.Playlists
            .Select(p => PlaylistConverter.DbToCoreModel(p)!)
            .ToListAsync();
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlistId);
        return PlaylistConverter.DbToCoreModel(playlistDbModel);
    }

    public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
    {
        return await _context.Playlists
            .Where(p => p.UserId == userId)
            .Select(p => PlaylistConverter.DbToCoreModel(p))
            .ToListAsync();
    }

    public async Task<Playlist> UpdatePlaylist(Playlist playlist)
    {
        var playlistDbModel = await _context.Playlists.FindAsync(playlist.Id);

        playlistDbModel!.Id = playlist.Id;
        playlistDbModel!.Title = playlist.Title;
        playlistDbModel!.UserId = playlist.UserId;

        await _context.SaveChangesAsync();
        return PlaylistConverter.DbToCoreModel(playlistDbModel);
    }
}