using Opah.ReportService.Application.Dtos;

namespace Opah.ReportService.Application.Services
{
    public interface IReportService
    {
        Task<ReportDailyDto> GetReport(ReportDailyParamsDto @params);
        Task<ReportDto> GetReport(ReportParamsDto @params);
    }
}
