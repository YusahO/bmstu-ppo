using MewingPad.Services.ScoreService;
using MewingPad.UI.DTOs;
using MewingPad.UI.DTOs.Converters;
using MewingPad.Utils.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController(IScoreService scoreService) : ControllerBase
{
    private readonly IScoreService _scoreService = scoreService
                                                   ?? throw new ArgumentNullException(nameof(scoreService));
    private readonly Serilog.ILogger _logger = Log.ForContext<ScoresController>();

    [AllowAnonymous]
    [HttpGet("{audiotrackId:guid}")]
    public async Task<IActionResult> GetAllScores(Guid audiotrackId)
    {
        try
        {
            var scores = from score in await _scoreService.GetAudiotrackScores(audiotrackId)
                         select ScoreConverter.CoreModelToDto(score);
            // log
            return Ok(scores);
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{authorId:guid}/{audiotrackId:guid}")]
    public async Task<IActionResult> GetScoreByPrimaryKey(Guid authorId, Guid audiotrackId)
    {
        try
        {
            var score = await _scoreService.GetScoreByPrimaryKey(authorId, audiotrackId);
            // log
            return Ok(ScoreConverter.CoreModelToDto(score));
        }
        catch (Exception ex)
        {
            // log
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateScore([FromBody] ScoreDto score)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            score.AuthorId = Guid.Parse(TokenUtils.TryGetClaimOfType(accessToken!));

            if ((await _scoreService.GetScoreByPrimaryKey(score.AuthorId, score.AudiotrackId)) is null)
            {
                await _scoreService.CreateScore(ScoreConverter.DtoToCoreModel(score));
                return Ok("Score created successfully");
            }
            else
            {
                await _scoreService.UpdateScore(ScoreConverter.DtoToCoreModel(score));
                return Ok("Score updated successfully");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}