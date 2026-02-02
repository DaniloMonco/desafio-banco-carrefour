using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Opah.TransactionOutbox.Infrastructure.Repositories
{
    public class DapperContext
    {
        private IDbConnection _connection { get; }
        public DapperContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSql")!;
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public IDbConnection GetConnection()
            => _connection;

    }
}
