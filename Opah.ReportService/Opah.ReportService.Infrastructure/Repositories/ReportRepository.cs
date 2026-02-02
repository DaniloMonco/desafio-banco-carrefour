using Dapper;
using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.Repositories;


namespace Opah.ReportService.Infrastructure.Repositories
{
    public class ReportRepository(DapperContext context) : IReportRepository
    {
        private readonly DapperContext _context = context;

        public Task<IEnumerable<Transaction>> GetTransactions(int year, int month, int day)
        {
            var sql = @"select Id, OccurredAt, Value, Description, TransactionType 
                        from business.transactions
                        where extract('YEAR' from OccurredAt) = @year
                        and extract('MONTH' from OccurredAt) = @month
                        and extract('DAY' from OccurredAt) = @day";

            return _context.CreateConnection().QueryAsync<Transaction>(sql, new { year, month, day });
        }

        public Task<IEnumerable<Transaction>> GetTransactions(int year, int month)
        {
            var sql = @"select Id, OccurredAt, Value, Description, TransactionType 
                        from business.transactions 
                        where extract('YEAR' from OccurredAt) = @year
                        and extract('MONTH' from OccurredAt) = @month";

            return _context.CreateConnection().QueryAsync<Transaction>(sql, new { year, month });
        }
    }
}
