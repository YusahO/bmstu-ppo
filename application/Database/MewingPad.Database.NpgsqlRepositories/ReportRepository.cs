using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.NpgsqlRepositories;

public class ReportRepository(MewingPadDbContext context) : IReportRepository
{
    private readonly MewingPadDbContext _context = context;

    private readonly ILogger _logger = Log.ForContext<ReportRepository>();

    public async Task AddReport(Report report)
    {
        _logger.Verbose("Entering AddReport method");

        try
        {
            await _context.Reports.AddAsync(ReportConverter.CoreToDbModel(report));
            await _context.SaveChangesAsync();
            _logger.Information("Added report ({@Report}) to database", report);
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            throw;
        }

        _logger.Verbose("Exiting AddReport method");
    }

    public async Task<List<Report>> GetAllReports()
    {
        _logger.Verbose("Entering GetAllReports method");
        
        var reports = await _context.Reports
            .Select(r => ReportConverter.DbToCoreModel(r))
            .ToListAsync();
        if (reports.Count == 0)
        {
            _logger.Warning("Database contains no entries of Report");
        }

        _logger.Verbose("Exiting GetAllReports method");
        return reports;
    }

    public async Task<Report?> GetReportById(Guid reportId)
    {
        _logger.Verbose("Entering GetReportById method");

        var reportDbModel = await _context.Reports.FindAsync(reportId);
        if (reportDbModel is null)
        {
            _logger.Warning($"Report (Id = {reportDbModel}) not found in database");
        }
        var report = ReportConverter.DbToCoreModel(reportDbModel);

        _logger.Verbose("Exiting GetReportById method");
        return report;
    }

    public async Task<Report> UpdateReport(Report report)
    {
        _logger.Verbose("Entering UpdateReport method");

        var reportDbModel = await _context.Reports.FindAsync(report.Id);

        reportDbModel!.AuthorId = report.AuthorId;
        reportDbModel!.AudiotrackId = report.AudiotrackId;
        reportDbModel!.Text = report.Text;
        reportDbModel!.Status = report.Status;

        await _context.SaveChangesAsync();
        _logger.Information($"Report (Id = {report.Id}) updated");
        _logger.Verbose("Exiting UpdateReport method");
        return report;
    }
}