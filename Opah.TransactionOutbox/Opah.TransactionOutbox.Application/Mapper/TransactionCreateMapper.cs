using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionOutbox.Application.Publisher.Message;
using Opah.TransactionOutbox.Domain.Entities;
using Opah.TransactionOutbox.Domain.Events;

namespace Opah.TransactionOutbox.Application.Mapper
{
    public static class TransactionCreateMapper
    {
        extension(TransactionCreatedEvent transactionEvent)
        {
            public TransactionCreateMessage ToMessage()
            => new TransactionCreateMessage(
                    transactionEvent.TransactionId,
                    transactionEvent.Amount,
                    (TransactionMessageType)transactionEvent.TransactionType,
                    transactionEvent.OccurredAt,
                    transactionEvent.UserName);
        }
    }
}
