using MewingPad.Common.Entities;
using MewingPad.Common.Exceptions;
using MewingPad.Common.IRepositories;
using MewingPad.Database.MongoDB.Context;
using MewingPad.Database.MongoDB.Models.Converters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MewingPad.Database.MongoDB.Repositories;

public class ReportRepository(MewingPadMongoDbContext context) : IReportRepository
{
	private readonly MewingPadMongoDbContext _context = context;
	private readonly ILogger _logger = Log.ForContext<ReportRepository>();

    public async Task AddReport(Report report)
    {
        _logger.Verbose("Entering AddReport");

        try
        {
            await _context.Reports.AddAsync(ReportConverter.CoreToDbModel(report));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting AddReport");
    }

    public async Task<List<Report>> GetAllReports()
    {
        _logger.Verbose("Entering GetAllReports");

        List<Report> reports;
        try
        {
            var found = await _context.Reports.ToListAsync();
            reports = found.Select(r => ReportConverter.DbToCoreModel(r)).ToList();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetAllReports");
        return reports;
    }

    public async Task<Report?> GetReportById(Guid reportId)
    {
        _logger.Verbose("Entering GetReportById");

        Report? report;
        try
        {
            var reportDbModel = await _context.Reports.FindAsync(reportId);
            report = ReportConverter.DbToCoreModel(reportDbModel);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting GetReportById");
        return report;
    }

    public async Task<Report> UpdateReport(Report report)
    {
        _logger.Verbose("Entering UpdateReport");

        try
        {
            var reportDbModel = await _context.Reports.FindAsync(report.Id);

            reportDbModel!.AuthorId = report.AuthorId;
            reportDbModel!.AudiotrackId = report.AudiotrackId;
            reportDbModel!.Text = report.Text;
            reportDbModel!.Status = report.Status;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex.InnerException);
        }

        _logger.Verbose("Exiting UpdateReport");
        return report;
    }
}
