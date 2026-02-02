using Dapper;
using Opah.ReportInbox.Domain.Entities;
using Opah.ReportInbox.Domain.Repositories;

namespace Opah.ReportInbox.Infrastructure.Repositories
{
    public class TransactionRepository(DapperContext context) : ITransactionRepository
    {
        protected readonly DapperContext _context = context;

        public Task Save(Transaction transaction)
        {
            var query = "INSERT INTO business.transactions(Id, OccurredAt, Value, Description, TransactionType) VALUES (:Id, :OccurredAt, :Value, :Description, :TransactionType)";

            return _context.GetConnection().ExecuteAsync(query, transaction);
        }
    }
}
