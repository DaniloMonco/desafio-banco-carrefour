using Opah.ReportInbox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task Save(Transaction entity);
    }
}
