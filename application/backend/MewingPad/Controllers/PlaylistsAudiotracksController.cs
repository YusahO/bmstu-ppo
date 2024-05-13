using MewingPad.Services.PlaylistService;
using MewingPad.UI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsAudiotracksController(IPlaylistService playlistService) : ControllerBase
{
    private readonly IPlaylistService _playlistService = playlistService;

    [Authorize]
    [HttpGet("{playlistId:guid}")]
    public async Task<IActionResult> GetPlaylistAudiotracks(Guid playlistId)
    {
        try
        {
            var audiotracks = await _playlistService.GetAllAudiotracksFromPlaylist(playlistId);
            return Ok(audiotracks);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPlaylistsWithAudiotrack([FromQuery] Guid userId,
                                                                [FromQuery] Guid audiotrackId)
    {
        try
        {
            var playlists = await _playlistService.GetUserPlaylistsContainingAudiotrack(userId, audiotrackId);
            return Ok(playlists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToPlaylist([FromBody] PlaylistAudiotrackDto paDto)
    {
        try
        {
            await _playlistService.AddAudiotrackToPlaylist(paDto.PlaylistId, paDto.AudiotrackId);
            return Ok("Audiotrack added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveFromPlaylist([FromBody] PlaylistAudiotrackDto paDto)
    {
        try
        {
            await _playlistService.RemoveAudiotrackFromPlaylist(paDto.PlaylistId, paDto.AudiotrackId);
            return Ok("Audiotrack removed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}