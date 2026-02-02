namespace Opah.ReportService.Application.Dtos
{
    public record ReportItemDto(DateOnly Date, decimal Debit, decimal Credit, decimal Total);
}
