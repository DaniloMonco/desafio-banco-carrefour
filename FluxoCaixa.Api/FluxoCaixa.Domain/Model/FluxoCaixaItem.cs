namespace FluxoCaixa.Domain.Model
{
    public class FluxoCaixaItem
    {
        public DateOnly Data { get; protected set; }
        public decimal Debito { get; protected set; }
        public decimal Credito { get; protected set; }
        public decimal Saldo => Credito - Debito;

        protected FluxoCaixaItem()
        {

        }

        protected FluxoCaixaItem(DateOnly data, decimal debito, decimal credito) 
        {
            Data = data;
            Debito = debito;
            Credito = credito;
        }

        public static FluxoCaixaItem Criar(DateOnly data, decimal debito, decimal credito)
            => new FluxoCaixaItem(data, debito, credito);
        public static FluxoCaixaItem Criar(DateOnly data)
            => new FluxoCaixaItem { Data = data };

        public void AdicionarValor(TipoLancamento tipoLancamento, decimal valor)
        {
            if (tipoLancamento == TipoLancamento.C)
                Credito += valor;
            else
                Debito += valor;
        }
    }
}
