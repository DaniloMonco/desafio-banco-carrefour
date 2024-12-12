using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Model
{
    public class FluxoCaixa
    {
        protected FluxoCaixa() { }
        protected FluxoCaixa(int ano, int mes)
        {
            Ano = ano;
            Mes = mes;
            Items = new List<FluxoCaixaItem>();
        }

        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal Saldo => Items.Sum(i=>i.Saldo);

        public IList<FluxoCaixaItem> Items { get; set; }

        public static FluxoCaixa Criar(int ano, int mes)
        {
            return new FluxoCaixa(ano, mes);
        }
        
        public void MontarFluxoCaixa(IEnumerable<Lancamento> lancamentos)
        {
            var lancamentosAgrupados = lancamentos.GroupBy(lancamento => new
            {
                lancamento.TipoLancamento,
                Data = lancamento.DataHora.Date
            }).Select(lancamentoGroup => new
            {
                lancamentoGroup.Key.Data,
                lancamentoGroup.Key.TipoLancamento,
                Valor = lancamentoGroup.Sum(lancamento => lancamento.Valor)
            }).OrderBy(lancamentoGroup => lancamentoGroup.Data);

            var fluxoCaixaDictionary = new Dictionary<DateTime, FluxoCaixaItem>();

            foreach(var lancamento in lancamentosAgrupados)
            {
                fluxoCaixaDictionary.TryGetValue(lancamento.Data, out var item);
                if (item is null)
                    item = FluxoCaixaItem.Criar(DateOnly.FromDateTime(lancamento.Data));

                item.AdicionarValor(lancamento.TipoLancamento, lancamento.Valor);

                fluxoCaixaDictionary[lancamento.Data] = item;
            }

            foreach(var fluxo in fluxoCaixaDictionary)
                Items.Add(FluxoCaixaItem.Criar(fluxo.Value.Data, fluxo.Value.Debito, fluxo.Value.Credito));
        }
        
    }
}
