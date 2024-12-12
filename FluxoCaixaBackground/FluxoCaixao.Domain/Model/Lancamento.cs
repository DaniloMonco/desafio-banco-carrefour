namespace FluxoCaixa.Domain.Model
{
    public abstract class Lancamento
    {
        protected Lancamento(Guid id, DateTime dataHora, decimal valor, string descricao)
        {
            Id = id;
            DataHora = dataHora;
            Valor = valor;
            Descricao = descricao;
            TipoLancamento = DefinirTipoLancamento();
        }
        public Guid Id { get; protected set; }
        public DateTime DataHora { get; protected set; }
        public decimal Valor { get; protected set; }
        public string Descricao { get; protected set; }
        public TipoLancamento TipoLancamento { get; private set; }
        protected abstract TipoLancamento DefinirTipoLancamento();
    }
}
