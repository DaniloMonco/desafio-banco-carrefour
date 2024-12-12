namespace ControleLancamento.Domain.Events
{
    public abstract class LancamentoEfetuadoEvent : DomainEvent
    {
        protected LancamentoEfetuadoEvent(Guid id, DateTime dataHora, decimal valor, string descricao)
        {
            LancamentoId = id;
            DataHora = dataHora;
            Valor = valor;
            Descricao = descricao;
        }

        public Guid LancamentoId { get; protected set; }
        public DateTime DataHora { get; protected set; }
        public decimal Valor { get; protected set; }
        public string Descricao { get; protected set; }
    }
}
