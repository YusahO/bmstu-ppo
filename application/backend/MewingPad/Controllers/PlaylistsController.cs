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
    [HttpDelete("{playlistId:guid}")]
    public async Task<IActionResult> DeletePlaylist(Guid playlistId)
    {
        try
        {
            await _playlistService.DeletePlaylist(playlistId);
            return Ok("Playlist deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{playlistId:guid}/audiotracks")]
    public async Task<IActionResult> RemoveFromPlaylist(Guid playlistId)
    {
        try
        {
            var audiotracks = from a in await _playlistService.GetAllAudiotracksFromPlaylist(playlistId)
                              select AudiotrackConverter.CoreModelToDto(a);
            return Ok(audiotracks);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{playlistId:guid}/audiotracks/{audiotrackId:guid}")]
    public async Task<IActionResult> RemoveFromPlaylist(Guid playlistId, Guid audiotrackId)
    {
        try
        {
            await _playlistService.RemoveAudiotrackFromPlaylist(playlistId, audiotrackId);
            return Ok("Audiotrack removed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{playlistId:guid}/audiotracks/{audiotrackId:guid}")]
    public async Task<IActionResult> AddToPlaylist(Guid playlistId, Guid audiotrackId)
    {
        try
        {
            await _playlistService.AddAudiotrackToPlaylist(playlistId, audiotrackId);
            return Ok("Audiotrack added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}