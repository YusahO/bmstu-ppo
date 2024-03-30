using MewingPad.Common.Entities;
using MewingPad.Common.IRepositories;
using MewingPad.Database.Context;
using MewingPad.Database.Models.Converters;

namespace MewingPad.Database.NpgsqlRepositories;

public class ReportRepository(MewingPadDbContext context) : IReportRepository
{
    private readonly MewingPadDbContext _context = context;

    public async Task AddReport(Report score)
    {
        await _context.Reports.AddAsync(ReportConverter.CoreToDbModel(score)!);
        await _context.SaveChangesAsync();
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
        reportDbModel!.AudiofileId = report.AudiofileId;
        reportDbModel!.Text = report.Text;
        reportDbModel!.Status = report.Status;

        await _context.SaveChangesAsync();
        return ReportConverter.DbToCoreModel(reportDbModel)!;
    }
}