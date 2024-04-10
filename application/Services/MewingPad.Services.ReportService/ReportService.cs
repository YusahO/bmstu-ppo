using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using Serilog;

namespace MewingPad.Services.ReportService;

public class ReportService(IReportRepository reportRepository) : IReportService
{
    private readonly IReportRepository _reportRepository = reportRepository;

    private readonly ILogger _logger = Log.ForContext<ReportService>();

    public async Task CreateReport(Report report)
    {
        _logger.Information("Entering CreateReport method");

        if (await _reportRepository.GetReportById(report.Id) is not null)
        {
            _logger.Error($"Report (Id = {report.Id}) already exists");
            throw new ReportExistsException(report.Id);
        }
        await _reportRepository.AddReport(report);

        _logger.Information("Exiting CreateReport method");
    }

    public async Task<Report> UpdateReportStatus(Guid reportId, ReportStatus status)
    {
        _logger.Information("Entering UpdateReportStatus method");

        var report = await _reportRepository.GetReportById(reportId);
        if (report is null)
        {
            _logger.Error($"Report (Id = {reportId}) not found");
            throw new ReportNotFoundException(reportId);
        }
        report.Status = status;
        await _reportRepository.UpdateReport(report);

        _logger.Information("Exiting UpdateReportStatus method");
        return report;
    }

    public async Task<Report> GetReportById(Guid reportId)
    {
        _logger.Information("Entering GetReportById method");

        var report = await _reportRepository.GetReportById(reportId);
        if (report is null)
        {
            _logger.Error($"Report (Id = {reportId}) not found");
            throw new ReportNotFoundException(reportId);
        }

        _logger.Information("Exiting GetReportById method");
        return report;
    }

    public async Task<List<Report>> GetAllReports()
    {
        _logger.Information("Entering GetAllReports method");
        var reports = await _reportRepository.GetAllReports();
        _logger.Information("Exiting GetAllReports method");
        return reports;
    }
}
