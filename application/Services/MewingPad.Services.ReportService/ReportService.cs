﻿using MewingPad.Common.Entities;
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
        _logger.Verbose("Entering CreateReport method");

        if (await _reportRepository.GetReportById(report.Id) is not null)
        {
            _logger.Error($"Report (Id = {report.Id}) already exists");
            throw new ReportExistsException(report.Id);
        }
        await _reportRepository.AddReport(report);

        _logger.Verbose("Exiting CreateReport method");
    }

    public async Task<Report> UpdateReportStatus(Guid reportId, ReportStatus status)
    {
        _logger.Verbose("Entering UpdateReportStatus method");

        var report = await _reportRepository.GetReportById(reportId);
        if (report is null)
        {
            _logger.Error($"Report (Id = {reportId}) not found");
            throw new ReportNotFoundException(reportId);
        }
        report.Status = status;
        await _reportRepository.UpdateReport(report);

        _logger.Verbose("Exiting UpdateReportStatus method");
        return report;
    }

    public async Task<Report> GetReportById(Guid reportId)
    {
        _logger.Verbose("Entering GetReportById method");

        var report = await _reportRepository.GetReportById(reportId);
        if (report is null)
        {
            _logger.Error($"Report (Id = {reportId}) not found");
            throw new ReportNotFoundException(reportId);
        }

        _logger.Verbose("Exiting GetReportById method");
        return report;
    }

    public async Task<List<Report>> GetAllReports()
    {
        _logger.Verbose("Entering GetAllReports method");
        var reports = await _reportRepository.GetAllReports();
        _logger.Verbose("Exiting GetAllReports method");
        return reports;
    }
}
