using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Application.Mappers;
using Opah.ReportService.Domain.Entities;

namespace Opah.ReportService.Application.Mappers
{
    public static class ReportMapper
    {
        extension(Report model)
        {
            public ReportDto? ToReportDto()
                => new ReportDto(model.ReferenceMonth.Year, model.ReferenceMonth.Month, 
                    model?.Items?.Select(i => new ReportItemDto(i.Date, i.Debit, i.Credit, i.Total)));

            public ReportDailyDto? ToReportDailyDto()
            {
                if (model is null || model.Items is null)
                    return null;

                var item = model.Items.First();
                return new ReportDailyDto(model.ReferenceMonth.Year, model.ReferenceMonth.Month,
                                         (UInt16)item.Date.Day, item.Debit, item.Credit, item.Total);
            }
        }
    }
}
