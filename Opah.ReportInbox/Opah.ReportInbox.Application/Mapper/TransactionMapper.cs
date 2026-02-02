using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Domain.Entities;
using Opah.ReportInbox.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Application.Mapper
{
    public static class TransactionMapper
    {
        extension(TransactionCreatedMessage transaction)
        {
            public Transaction ToEntity()
            => new Transaction(transaction.TransactionId, transaction.OccurredAt,
                transaction.Amount, string.Empty, (TransactionType)transaction.TransactionType);
        }
    }
}
