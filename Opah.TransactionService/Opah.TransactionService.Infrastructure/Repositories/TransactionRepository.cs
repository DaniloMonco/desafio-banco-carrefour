using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Opah.TransactionService.Domain.Entities;
using Opah.TransactionService.Domain.Repositories;

namespace Opah.TransactionService.Infrastructure.Repositories
{
    public class TransactionRepository(DapperContext context) : ITransactionRepository
    {
        protected readonly DapperContext _context = context;

        public async Task Save(Transaction transaction)
        {
            var query = "INSERT INTO business.transactions(TransactionId, Amount, Type, OccurredAt) VALUES (:Id, :Amount, :Type, :OccurredAt)";

            await _context.GetConnection().ExecuteAsync(query, transaction);
        }
    }
}
