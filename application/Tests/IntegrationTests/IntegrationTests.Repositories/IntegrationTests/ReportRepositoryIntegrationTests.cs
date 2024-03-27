using IntegrationTests.DbFixtures;
using MewingPad.Common.Entities;

namespace IntegrationTests.Repositories;

public class ReportRepositoryIntegrationTests : IDisposable
{
    private readonly InMemoryDbFixture _dbFixture = new();

    [Fact]
    public async Task TestCreateReport()
    {
        var expectedReport = new Report(Guid.NewGuid(), Guid.Empty, Guid.Empty, "");
        
        await _dbFixture.ReportRepository.AddReport(expectedReport);
        var actualReport = await _dbFixture.ReportRepository.GetReportById(expectedReport.Id);

        Assert.Equal(expectedReport, actualReport);
    }

    [Fact]
    public async Task TestGetReportById()
    {
        var reports = InMemoryDbFixture.CreateMockReports();
        await _dbFixture.InsertReports(reports);

        var expectedReport = new Report(reports.First());

        var actualReport = await _dbFixture.ReportRepository.GetReportById(expectedReport.Id);

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

        var actualReport = await _dbFixture.ReportRepository.UpdateReport(expectedReport);

        Assert.Equal(expectedReport, actualReport);
    }

    public void Dispose() => _dbFixture.Dispose();
}