using MewingPad.Services.ReportService;
using MewingPad.UI.DTOs;
using MewingPad.UI.DTOs.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MewingPad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllReports()
    {
        try
        {
            var reports = await _reportService.GetAllReports();
            return Ok(reports);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddReport([FromBody] ReportDto reportDto)
    {
        try
        {
            var report = ReportConverter.DtoToCoreModel(reportDto);
            report.Id = Guid.NewGuid();
            await _reportService.CreateReport(report);
            return Ok("Report added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateReport([FromBody] ReportDto reportDto)
    {
        try
        {
            var report = ReportConverter.DtoToCoreModel(reportDto);
            await _reportService.UpdateReportStatus(report.Id, report.Status);
            return Ok("Report updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}