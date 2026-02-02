using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionService.Domain.ValueObject;

namespace Opah.TransactionService.Domain.Events
{

    public class TransactionCreated : TransactionEvent
    {
        public TransactionCreated(Guid transactionId, decimal amount, TransactionType transactionType, DateTime occurredAt, string? userName) 
            : base(transactionId, amount, transactionType, occurredAt, userName)
        {
        }
    }
}
