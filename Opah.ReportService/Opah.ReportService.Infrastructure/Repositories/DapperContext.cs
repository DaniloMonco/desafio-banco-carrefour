using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Opah.ReportService.Infrastructure.Repositories
{
    public class DapperContext(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("PostgreSql")!;

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
