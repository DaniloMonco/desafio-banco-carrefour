using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Application.Consumer.Message
{
    public record TransactionCreatedMessage(Guid TransactionId, decimal Amount, TransactionTypeMessage TransactionType, DateTime OccurredAt);
}
