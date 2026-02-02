using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Application.Mappers;
using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.Repositories;
using Opah.ReportService.Domain.ValueObjects;

namespace Opah.ReportService.Application.Services
{
    public class ReportService(IReportRepository repository) : IReportService
    {
        private readonly IReportRepository _repository = repository;
        
        public virtual async Task<ReportDailyDto> GetReport(ReportDailyParamsDto @params)
        {
            var year = @params.Year;
            var month = @params.Month;
            var day = @params.Day;

            var transactions = await _repository.GetTransactions(year, month, day);
            var reportModel = BuildReport(year, month, transactions);
            return reportModel.ToReportDailyDto()!;
        }

        public virtual async Task<ReportDto> GetReport(ReportParamsDto @params)
        {
            var year = @params.Year;
            var month = @params.Month;

            var transactions = await _repository.GetTransactions(year, month);
            var reportModel = BuildReport(year, month, transactions);
            return reportModel.ToReportDto()!;
        }

        private Report BuildReport(UInt16 year, UInt16 month, IEnumerable<Transaction> transactions)
        {
            var report = new Report(new ReferenceMonth(year, month));
            report.Build(transactions);
            return report;
        }

    }

   
}
