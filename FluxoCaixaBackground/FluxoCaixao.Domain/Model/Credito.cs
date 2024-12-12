namespace FluxoCaixa.Domain.Model
{
    public class Credito : Lancamento
    {
        public Credito(Guid id, DateTime dataHora, decimal valor, string descricao)
            : base(id, dataHora, valor, descricao)
        {
        }

        protected override TipoLancamento DefinirTipoLancamento() => TipoLancamento.C;
    }
}
