using Dapper;
using FluxoCaixa.Domain.Model;
using FluxoCaixa.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Repository
{
    public class LancamentoRepository : ILancamentoRepository
    {
        public readonly IDbConnection _connection;
        public LancamentoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<IEnumerable<Lancamento>> RecuperarLancamentos(int ano, int mes, int dia)
        {
            var sql = @"select * from lancamentos 
                        where extract('YEAR' from datahora) = @ano
                        and extract('MONTH' from datahora) = @mes
                        and extract('DAY' from datahora) = @dia";

            return _connection.QueryAsync<Lancamento>(sql, new { ano, mes, dia });
        }

        public Task<IEnumerable<Lancamento>> RecuperarLancamentos(int ano, int mes)
        {
            var sql = @"select * from lancamentos 
                        where extract('YEAR' from datahora) = @ano
                        and extract('MONTH' from datahora) = @mes";

            return _connection.QueryAsync<Lancamento>(sql, new { ano, mes});
        }
    }
}
