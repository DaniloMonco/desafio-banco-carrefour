using ControleLancamento.Domain.Model;

namespace ControleLancamento.Domain.Events
{
    public class DebitoLancadoEvent : LancamentoEfetuadoEvent
    {
        public DebitoLancadoEvent(Guid debitoId, DateTime dataHora, decimal valor, string descricao) 
            : base(debitoId, dataHora, valor, descricao)
        {
        }

        public static DebitoLancadoEvent Criar(Debito debito)
        {
            return new DebitoLancadoEvent(debito.Id, debito.DataHora, debito.Valor, debito.Descricao);
        }
    }
}
