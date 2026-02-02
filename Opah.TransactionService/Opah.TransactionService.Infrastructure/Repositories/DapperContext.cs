using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Opah.TransactionService.Infrastructure.Repositories
{
    public sealed class DapperContext : IDisposable
    {
        private IDbConnection _connection { get; }
        private IDbTransaction? _transaction { get; set; }
        public DapperContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSql")!;
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public IDbConnection GetConnection()
            => _connection;

        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public void Commit()
        {
            if (_transaction is null)
                throw new InvalidOperationException("IDbTransaction is null.");

            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            if (_transaction is null)
                throw new InvalidOperationException("IDbTransaction is null.");
            _transaction.Rollback();
            Dispose();
        }
        public void Dispose()
        {
            _connection?.Dispose();
            _transaction?.Dispose();
        }
    }
}
