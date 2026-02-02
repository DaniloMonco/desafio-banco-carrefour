using Dapper;
using Newtonsoft.Json;
using System.Data;
using Opah.TransactionService.Domain.Events;
using Opah.TransactionService.Domain.Repositories;
using Opah.TransactionService.Infrastructure.Outbox;

namespace Opah.TransactionService.Infrastructure.Repositories
{
    public class TransactionCreatedOutboxRepository(DapperContext context) : ITransactionCreatedOutboxRepository
    {
        protected readonly DapperContext _context = context;

        public async Task Save(IEnumerable<TransactionEvent> domainEvents)
        {
            foreach (var @event in domainEvents)
            {
                var outboxMessage = new TransactionOutbox(JsonConvert.SerializeObject(@event));

                var query = "INSERT INTO outbox.transactionscreated(Id, OccurredOnUtc, Payload) VALUES (:Id, :OccurredOnUtc, :Payload)";

                await _context.GetConnection().ExecuteAsync(query, outboxMessage);
            }
        }
    }
}
