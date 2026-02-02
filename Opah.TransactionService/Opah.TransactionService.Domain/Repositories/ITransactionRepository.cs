using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionService.Domain.Entities;

namespace Opah.TransactionService.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task Save(Transaction transaction);
    }
}
