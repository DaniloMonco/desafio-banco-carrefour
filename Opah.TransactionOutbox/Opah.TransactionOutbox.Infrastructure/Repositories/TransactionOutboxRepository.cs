using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.RateLimiting;
using Opah.TransactionOutbox.Domain.Entities;
using Opah.TransactionOutbox.Domain.Repositories;

namespace Opah.TransactionOutbox.Infrastructure.Repositories
{

    public class TransactionOutboxRepository(DapperContext context) : ITransactionOutboxRepository
    {
        protected readonly DapperContext _context = context;

        public Task Processed(TransactionCreated outbox)
        {
            var sqlCommand = "UPDATE outbox.transactionscreated SET ProcessedOnUtc = :ProcessedOnUtc WHERE Id = :Id";

            return _context.GetConnection().ExecuteAsync(sqlCommand, outbox);
        }

        public Task<IEnumerable<TransactionCreated>> GetTransactionsCreated()
        {
            var query = $"SELECT Id, OccurredOnUtc, ProcessedOnUtc, Payload FROM outbox.transactionscreated WHERE ProcessedOnUtc IS NULL ORDER BY OccurredOnUtc LIMIT 50 FOR UPDATE SKIP locked";

            return _context.GetConnection().QueryAsync<TransactionCreated>(query);
        }
    }
}
