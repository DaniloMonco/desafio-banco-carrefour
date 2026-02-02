using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionOutbox.Domain.Events
{
    public record TransactionCreatedEvent(Guid TransactionId, decimal Amount, TransactionType TransactionType, DateTime OccurredAt, string? UserName);
}
