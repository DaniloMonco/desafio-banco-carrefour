using Dapper;
using Npgsql;
using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Application.Idempotency;

namespace Opah.ReportInbox.Infrastructure.Repositories
{
    public class TransactionCreatedRepository : ITransactionCreatedIdempotency
    {
        protected readonly DapperContext _context;
        public TransactionCreatedRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task VerifyAlreadyProcessed(TransactionCreatedMessage transaction)
        {
            var query = "INSERT INTO inbox.transactions(Id, ProcessedAt) VALUES (:Id, :ProcessedAt)";

            try
            {
                await _context.GetConnection().ExecuteAsync(query, new
                {
                    Id = transaction.TransactionId,
                    ProcessedAt = DateTime.Now
                });
            }
            catch (PostgresException ex)
            {
                if (((string)ex.Data["SqlState"]!) == "23505")
                    throw new IdempotencyException();
                
                throw;
            }
        }

    }
}
