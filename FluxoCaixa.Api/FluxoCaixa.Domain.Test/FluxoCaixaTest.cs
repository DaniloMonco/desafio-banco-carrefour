using FluxoCaixa.Domain.Model;

namespace FluxoCaixa.Domain.Test
{
    public class FluxoCaixaTest
    {
        private const int _ano = 2024;
        private const int _mes = 8;
        private const int _dia10 = 10;
        private const int _dia11 = 11;

        [Fact]
        public void Montar_FluxoCaixa_Com_Lancamentos()
        {
            var dataHora10 = new DateTime(_ano, _mes, _dia10);
            var dataHora11 = new DateTime(_ano, _mes, _dia11);
            var lancamentos = new List<Lancamento>
            {
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 1", TipoLancamento = TipoLancamento.D, Valor=10},
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 2", TipoLancamento = TipoLancamento.D, Valor=20},
                new Lancamento{DataHora=dataHora10, Descricao = "Credito 1", TipoLancamento = TipoLancamento.C, Valor=20},
                new Lancamento{DataHora=dataHora11, Descricao = "Credito 2", TipoLancamento = TipoLancamento.C, Valor=100},
                new Lancamento{DataHora=dataHora11, Descricao = "Debito 3", TipoLancamento = TipoLancamento.D, Valor=50},
            };

            var fluxoCaixa = Model.FluxoCaixa.Criar(_ano, _mes);
            fluxoCaixa.MontarFluxoCaixa(lancamentos);

            Assert.Equal(_ano, fluxoCaixa.Ano);
            Assert.Equal(_mes, fluxoCaixa.Mes);
            Assert.Equal(40, fluxoCaixa.Saldo);
            Assert.Equal(2, fluxoCaixa.Items.Count);
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), fluxoCaixa.Items[0].Data);
            Assert.Equal(30, fluxoCaixa.Items[0].Debito);
            Assert.Equal(20, fluxoCaixa.Items[0].Credito);
            Assert.Equal(-10, fluxoCaixa.Items[0].Saldo);
            Assert.Equal(new DateOnly(_ano, _mes, _dia11), fluxoCaixa.Items[1].Data);
            Assert.Equal(50, fluxoCaixa.Items[1].Debito);
            Assert.Equal(100, fluxoCaixa.Items[1].Credito);
            Assert.Equal(50, fluxoCaixa.Items[1].Saldo);
        }
    }
}