using Opah.ReportService.Domain.ValueObjects;

namespace Opah.ReportService.Domain.Entities
{
        public record ReportItem(DateOnly Date, decimal Debit, decimal Credit)
        {
            public DateOnly Date { get; protected set; } = Date;
            public decimal Debit { get; protected set; } = Debit;
            public decimal Credit { get; protected set; } = Credit;

            public decimal Total => Credit - Debit;

            public ReportItem(DateOnly date) : this(date, default, default)
            {
            
            }

            public void AdicionarValor(TransactionType transactionType, decimal value)
            {
                if (transactionType == TransactionType.C)
                    Credit += value;
                else
                    Debit += value;
            }
        }
}
