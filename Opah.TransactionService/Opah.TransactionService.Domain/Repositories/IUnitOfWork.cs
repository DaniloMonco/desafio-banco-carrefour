using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionService.Domain.Repositories;

namespace Opah.TransactionService.Application.Persistence
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
