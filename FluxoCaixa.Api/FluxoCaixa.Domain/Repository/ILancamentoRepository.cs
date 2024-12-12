using FluxoCaixa.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Repository
{
    public interface ILancamentoRepository
    {
        Task<IEnumerable<Lancamento>> RecuperarLancamentos(int ano, int mes, int dia);

        Task<IEnumerable<Lancamento>> RecuperarLancamentos(int ano, int mes);
    }
}
