using Opah.ReportInbox.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Domain.Entities
{
    public record Transaction(Guid Id, DateTime OccurredAt, decimal Value, string Description, TransactionType TransactionType);
}
