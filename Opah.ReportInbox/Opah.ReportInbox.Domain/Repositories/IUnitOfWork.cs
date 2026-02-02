using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Domain.Repositories
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
