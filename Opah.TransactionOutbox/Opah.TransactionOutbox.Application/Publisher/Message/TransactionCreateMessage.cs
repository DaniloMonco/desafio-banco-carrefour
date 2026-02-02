using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionOutbox.Application.Publisher.Message
{
    public record TransactionCreateMessage(Guid TransactionId, decimal Amount, TransactionMessageType TransactionType, DateTime OccurredAt, string? UserName);

}
