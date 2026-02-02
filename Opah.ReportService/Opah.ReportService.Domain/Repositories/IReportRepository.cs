using Opah.ReportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Transaction>> GetTransactions(int year, int month, int day);

        Task<IEnumerable<Transaction>> GetTransactions(int year, int month);
    }
}
