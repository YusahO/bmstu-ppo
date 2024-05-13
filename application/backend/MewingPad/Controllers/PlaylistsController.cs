using MewingPad.Services.PlaylistService;
using MewingPad.UI.DTOs;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController(IPlaylistService playlistService) : ControllerBase
{
    private readonly IPlaylistService _playlistService = playlistService;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddPlaylist([FromBody] PlaylistDto playlistDto)
    {
        try
        {
            var playlist = PlaylistConverter.DtoToCoreModel(playlistDto);
            playlist.Id = Guid.NewGuid();
            await _playlistService.CreatePlaylist(playlist);
            return Ok("Playlist added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdatePlaylist([FromBody] PlaylistDto playlistDto)
    {
        try
        {
            var playlist = PlaylistConverter.DtoToCoreModel(playlistDto);
            await _playlistService.UpdatePlaylistTitle(playlist.Id, playlist.Title);
            return Ok("Playlist updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeletePlaylist([FromBody] PlaylistDto playlistDto)
    {
        try
        {
            var playlist = PlaylistConverter.DtoToCoreModel(playlistDto);
            await _playlistService.DeletePlaylist(playlist.Id);
            return Ok("Playlist deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserPlaylists(Guid userId)
    {
        try
        {
            var playlists = await _playlistService.GetUserPlaylists(userId);
            return Ok(playlists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}