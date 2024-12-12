using Dapper;
using FluxoCaixa.Domain.DAO;
using FluxoCaixa.Domain.Model;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace FluxoCaixa.Infrastructure.DAO
{
    public class LancamentoDao : ILancamentoDao
    {
        private readonly DbProviderFactory _dbFactory;
        private readonly string _connstr;

        public LancamentoDao(DbProviderFactory factory, IConfiguration config)
        {
            _connstr = config.GetConnectionString("PostgreSql");
            _dbFactory = factory;
        }

        public async Task<int> Gravar(Lancamento lancamento)
        {
            var connection = _dbFactory.CreateConnection();
            connection.ConnectionString = _connstr;

            var sql = "insert into Lancamentos values (@Id, @DataHora, @Valor, @Descricao, @TipoLancamento)";
            var rowsAffected = connection.Execute(sql, new
            {
                lancamento.Id,
                lancamento.DataHora,
                lancamento.Valor,
                lancamento.Descricao,
                TipoLancamento = lancamento.TipoLancamento.ToString()
            });

            return rowsAffected;
        }

    }
}
