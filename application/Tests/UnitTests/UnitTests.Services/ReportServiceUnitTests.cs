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
        List<Report> reps = [];
        var expectedRep = CreateMockReport();

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                              .ReturnsAsync(default(Report)!);

        _mockReportRepository.Setup(s => s.AddReport(expectedRep))
                             .Callback((Report r) => reps.Add(r));

        await _reportService.CreateReport(expectedRep);
        var actualRep = reps.Last();

        Assert.Equal(expectedRep, actualRep);
    }

    [Fact]
    public async Task TestCreateExistentReport()
    {
        var expectedRep = CreateMockReport();

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                             .ReturnsAsync(expectedRep);

        async Task Action() => await _reportService.CreateReport(expectedRep);

        await Assert.ThrowsAsync<ReportExistsException>(Action);
    }

    [Fact]
    public async Task TestUpdateReport()
    {
        var expectedRep = CreateMockReport();

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                              .ReturnsAsync(expectedRep);

        _mockReportRepository.Setup(s => s.UpdateReport(expectedRep))
                             .ReturnsAsync(expectedRep);

        var actualRep = await _reportService.UpdateReportStatus(expectedRep.Id, ReportStatus.Declined);

        Assert.Equal(expectedRep, actualRep);
    }

    [Fact]
    public async Task TestUpdateReportNonexistent()
    {
        _mockReportRepository.Setup(s => s.GetReportById(It.IsAny<Guid>()))
                                 .ReturnsAsync(default(Report)!);

        async Task Action() => await _reportService.UpdateReportStatus(Guid.Empty, ReportStatus.Declined);

        await Assert.ThrowsAsync<ReportNotFoundException>(Action);
    }

    [Fact]
    public async Task TestGetReportById()
    {
        var expectedRep = CreateMockReport();

        _mockReportRepository.Setup(s => s.GetReportById(expectedRep.Id))
                                 .ReturnsAsync(expectedRep);
        
        var actualRep = await _reportService.GetReportById(expectedRep.Id);

        Assert.Equal(expectedRep, actualRep);
    }

    [Fact]
    public async Task TestGetReportNonexistentById()
    {        
        _mockReportRepository.Setup(s => s.GetReportById(It.IsAny<Guid>()))
                             .ReturnsAsync(default(Report)!);
        
        async Task Action() => await _reportService.GetReportById(Guid.Empty);

        await Assert.ThrowsAsync<ReportNotFoundException>(Action);
    }

    private static Report CreateMockReport(ReportStatus status = ReportStatus.NotViewed)
    {
        return new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "whatever1", status);
    }
}