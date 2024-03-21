using MewingPad.Common.Enums;
using MewingPad.Common.Exceptions;
using MewingPad.Services.ReportService;

namespace UnitTests.Services;

public class ReportServiceUnitTest
{
    private readonly IReportService _reportService;
    private readonly Mock<IReportRepository> _mockReportRepository = new();

    public ReportServiceUnitTest()
    {
        _reportService = new ReportService(_mockReportRepository.Object);
    }

    [Fact]
    public async Task TestCreateReport()
    {
        var reps = CreateMockReports();
        var expectedRep = new Report(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "text");

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                              .ReturnsAsync(reps.Find(r => r.Id == expectedRep.Id)!);

        _mockReportRepository.Setup(s => s.AddReport(It.IsAny<Report>()))
                             .Callback((Report r) => reps.Add(r));

        await _reportService.CreateReport(expectedRep);
        var actualRep = reps.Last();

        Assert.Equal(expectedRep, actualRep);
    }

    [Fact]
    public async Task TestCreateExistentReport()
    {
        var reps = CreateMockReports();
        var expectedRep = new Report(reps[1]);

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                             .ReturnsAsync(expectedRep);

        async Task Action() => await _reportService.CreateReport(expectedRep);

        await Assert.ThrowsAsync<ReportExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateReport()
    {
        Report expected = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever1", ReportStatus.Accepted);
        expected.Status = ReportStatus.Viewed;

        _mockReportRepository.Setup(s => s.GetReportById(expected.Id))
                              .ReturnsAsync(expected);

        _mockReportRepository.Setup(s => s.UpdateReport(It.IsAny<Report>()))
                             .ReturnsAsync(expected);

        var actual = await _reportService.UpdateReportStatus(expected.Id, expected.Status);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task TestUpdateReportNonexistent()
    {
        var reps = CreateMockReports();
        var expected = new Report(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "text");

        _mockReportRepository.Setup(s => s.GetReportById(expected.Id))
                                 .ReturnsAsync(reps.Find(f => f.Id == expected.Id)!);

        async Task Action() => await _reportService.UpdateReportStatus(expected.Id, ReportStatus.Declined);

        await Assert.ThrowsAsync<ReportNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetReportById()
    {
        var reps = CreateMockReports();
        var expectedRep = reps[0];

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                                 .ReturnsAsync(reps.Find(f => f.Id == expectedRep.Id)!);
        
        var comm = await _reportService.GetReportById(expectedRep.Id);

        Assert.Equal(expectedRep, comm);
    }

    [Fact]
    public async Task TestGetReportNonexistentById()
    {
        var reps = CreateMockReports();
        var expectedRep = new Report(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "new report");
        
        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                             .ReturnsAsync(reps.Find(f => f.Id == expectedRep.Id)!);
        
        async Task Action() => await _reportService.GetReportById(expectedRep.Id);

        await Assert.ThrowsAsync<ReportNotFoundException>(Action);
    }

    private static List<Report> CreateMockReports()
    {
        return
        [
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever1", ReportStatus.Accepted),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever2", ReportStatus.Viewed),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever3"),
        ];
    }
}