using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;
using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.NpgsqlRepositories;

public class ReportRepository(MewingPadDbContext context) : IReportRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddReport(Report report)
    {
        await _context.Reports.AddAsync(ReportConverter.CoreToDbModel(report));
        await _context.SaveChangesAsync();
    }

    public async Task<List<Report>> GetAllReports()
    {
        return await _context.Reports
            .Select(r => ReportConverter.DbToCoreModel(r))
            .ToListAsync();
    }

    public async Task<Report?> GetReportById(Guid reportId)
    {
        var reportDbModel = await _context.Reports.FindAsync(reportId);
        return ReportConverter.DbToCoreModel(reportDbModel);
    }

    public async Task<Report> UpdateReport(Report report)
    {
        var reportDbModel = await _context.Reports.FindAsync(report.Id);

        reportDbModel!.AuthorId = report.AuthorId;
        reportDbModel!.AudiotrackId = report.AudiotrackId;
        reportDbModel!.Text = report.Text;
        reportDbModel!.Status = report.Status;

        await _context.SaveChangesAsync();
        return ReportConverter.DbToCoreModel(reportDbModel);
    }
}