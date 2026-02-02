namespace Opah.ReportService.Application.Dtos
{
    public record ReportDailyDto(UInt16 Year, UInt16 Month, UInt16 Day, decimal Debit, decimal Credit, decimal Total);
}
