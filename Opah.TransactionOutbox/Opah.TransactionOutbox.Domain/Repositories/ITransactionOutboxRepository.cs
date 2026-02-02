using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionOutbox.Domain.Entities;

namespace Opah.TransactionOutbox.Domain.Repositories
{
    public interface ITransactionOutboxRepository
    {
        Task Processed(TransactionCreated outbox);
        Task<IEnumerable<TransactionCreated>> GetTransactionsCreated();
    }
}
