using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Model;
using System.Xml.Schema;

namespace ControleLancamento.Domain.Test
{
    public class LancamentoEfetuadoEventTest
    {
        [Fact]
        public void Retorna_Credito_Quando_Lancamento_Efetuado()
        {
            var creditoDataHora = DateTime.Now;
            var credito = Model.Credito.Lancar(creditoDataHora, 10, "Descricao");
            var eventos = credito.RecuperarEventos();

            Assert.Single(eventos);
            Assert.Equal(creditoDataHora, credito.DataHora);
            Assert.Equal("Descricao", credito.Descricao);
            Assert.Equal(TipoLancamento.C, credito.TipoLancamento);
            Assert.Equal(10, credito.Valor);
        }

        [Fact]
        public void Retorna_Debito_Quando_Lancamento_Efetuado()
        {
            var debitoDataHora = DateTime.Now;
            var debito = Model.Debito.Lancar(debitoDataHora, 10, "Descricao");
            var eventos = debito.RecuperarEventos();

            Assert.Single(eventos);
            Assert.Equal(debitoDataHora, debito.DataHora);
            Assert.Equal("Descricao", debito.Descricao);
            Assert.Equal(TipoLancamento.D, debito.TipoLancamento);
            Assert.Equal(10, debito.Valor);
        }

        [Fact]
        public void Retorna_CreditoLancadoEvent_Quando_Lancamento_Efetuado()
        {
            var credito = Model.Credito.Lancar(DateTime.Now, 10, "Descricao");
            var eventos = credito.RecuperarEventos();

            Assert.Single(eventos);

            var creditoLancadoEvent = (CreditoLancadoEvent)eventos.First();
            Assert.Equal(credito.DataHora, creditoLancadoEvent.DataHora);
            Assert.Equal(credito.Descricao, creditoLancadoEvent.Descricao);
            Assert.Equal(credito.Id, creditoLancadoEvent.LancamentoId);
            Assert.Equal(credito.Valor, creditoLancadoEvent.Valor);
        }

        [Fact]
        public void Retorna_DebitoLancadoEvent_Quando_Lancamento_Efetuado()
        {
            var debito = Model.Debito.Lancar(DateTime.Now, 10, "Descricao");
            var eventos = debito.RecuperarEventos();

            Assert.Single(eventos);

            var debitoLancadoEvent = (DebitoLancadoEvent)eventos.First();
            Assert.Equal(debito.DataHora, debitoLancadoEvent.DataHora);
            Assert.Equal(debito.Descricao, debitoLancadoEvent.Descricao);
            Assert.Equal(debito.Id, debitoLancadoEvent.LancamentoId);
            Assert.Equal(debito.Valor, debitoLancadoEvent.Valor);
        }
    }
}