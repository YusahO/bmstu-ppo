using MewingPad.Services.ScoreService;
using MewingPad.UI.DTOs.Converters;
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
}