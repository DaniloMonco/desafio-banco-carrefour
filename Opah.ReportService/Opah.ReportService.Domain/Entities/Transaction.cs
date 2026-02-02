using Opah.ReportService.Domain.ValueObjects;

namespace Opah.ReportService.Domain.Entities
{
    public record Transaction(Guid Id, DateTime OccurredAt, decimal Value, string Description, TransactionType TransactionType);
}
