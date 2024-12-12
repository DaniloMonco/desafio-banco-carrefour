using FluxoCaixa.Domain.Model;

namespace FluxoCaixa.Domain.DAO
{
    public interface ILancamentoDao
    {
        Task<int> Gravar(Lancamento lancamento);
    }
}
