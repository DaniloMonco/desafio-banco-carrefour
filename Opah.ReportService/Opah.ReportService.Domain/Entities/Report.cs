using Opah.ReportService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Domain.Entities
{
    public record Report(ReferenceMonth ReferenceMonth)
    {
        public IEnumerable<ReportItem>? Items { get; protected set; }


        public void Build(IEnumerable<Transaction> transactions)
        {
            var report = new Dictionary<DateOnly, ReportItem>();

            foreach (var t in transactions)
            {
                var date = DateOnly.FromDateTime(t.OccurredAt);

                if (!report.TryGetValue(date, out var item))
                {
                    item = new ReportItem(date);
                    report.Add(date, item);
                }

                item.AdicionarValor(t.TransactionType, t.Value);
            }

            Items = report
                .OrderBy(x => x.Key)
                .Select(x => new ReportItem(
                    x.Value.Date,
                    x.Value.Debit,
                    x.Value.Credit))
                ;
        }
    }
}
