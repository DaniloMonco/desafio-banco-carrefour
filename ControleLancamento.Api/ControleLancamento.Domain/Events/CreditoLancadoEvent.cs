using ControleLancamento.Domain.Model;

namespace ControleLancamento.Domain.Events
{
    public class CreditoLancadoEvent : LancamentoEfetuadoEvent
    {
        public CreditoLancadoEvent(Guid creditoId, DateTime dataHora, decimal valor, string descricao)
            : base(creditoId, dataHora, valor, descricao)
        {
        }

        public static CreditoLancadoEvent Criar(Credito credito)
        {
            return new CreditoLancadoEvent(credito.Id, credito.DataHora, credito.Valor, credito.Descricao);
        }
    }
}
