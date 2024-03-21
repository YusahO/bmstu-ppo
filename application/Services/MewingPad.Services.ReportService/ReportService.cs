using MewingPad.Common.Entities;
using MewingPad.Common.Enums;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;

namespace MewingPad.Services.ReportService;

public class ReportService(IReportRepository reportRepository) : IReportService
{
    private readonly IReportRepository _reportRepository = reportRepository;

    public async Task CreateReport(Report report)
    {
        if (await _reportRepository.GetReportById(report.Id) is not null)
        {
            throw new ReportExistsException(report.Id);
        }
        await _reportRepository.AddReport(report);
    }

    public async Task<Report> UpdateReportStatus(Guid reportId, ReportStatus status)
    {
        var report = await _reportRepository.GetReportById(reportId)
                     ?? throw new ReportNotFoundException(reportId);
        report.Status = status;
        return await _reportRepository.UpdateReport(report);
    }

    public async Task<Report> GetReportById(Guid reportId)
    {
        return await _reportRepository.GetReportById(reportId) ?? throw new ReportNotFoundException(reportId);
    }
}
