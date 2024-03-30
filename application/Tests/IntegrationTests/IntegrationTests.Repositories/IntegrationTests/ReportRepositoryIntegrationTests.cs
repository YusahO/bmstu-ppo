using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;
using MewingPad.Database.NpgsqlRepositories;

namespace IntegrationTests.Repositories;

public class ReportRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();
    private readonly ReportRepository _reportRepository;

    public ReportRepositoryIntegrationTests()
    {
        _reportRepository = new ReportRepository(_dbFixture.Context);
    }

    [Fact]
    public async Task TestCreateReport()
    {
        var expectedReport = new Report(Guid.NewGuid(), Guid.Empty, Guid.Empty, "");
        
        await _reportRepository.AddReport(expectedReport);
        var actualReport = await _dbFixture.GetReportById(expectedReport.Id);

        Assert.Equal(expectedReport, actualReport);
    }

    [Fact]
    public async Task TestGetReportById()
    {
        var reports = InMemoryDbFixture.CreateMockReports();
        await _dbFixture.InsertReports(reports);

        var expectedReport = new Report(reports.First());

        var actualReport = await _reportRepository.GetReportById(expectedReport.Id);

        Assert.Equal(expectedReport, actualReport);
    }

    [Fact]
    public async Task TestUpdateReport()
    {
        var reports = InMemoryDbFixture.CreateMockReports();
        await _dbFixture.InsertReports(reports);

        var expectedReport = new Report(reports.First())
        {
            Text = "new text"
        };

        var actualReport = await _reportRepository.UpdateReport(expectedReport);

        Assert.Equal(expectedReport, actualReport);
    }

    public void Dispose() => _dbFixture.Dispose();
}