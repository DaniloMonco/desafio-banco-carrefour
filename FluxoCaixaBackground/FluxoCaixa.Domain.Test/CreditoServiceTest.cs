using FluxoCaixa.Domain.DAO;
using FluxoCaixa.Domain.Model;
using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.DomainServices;
using NSubstitute;

namespace FluxoCaixa.Domain.Test
{

    public class CreditoServiceTest
    {
        private ILancamentoDao _dao;
        public CreditoServiceTest()
        {
            _dao = Substitute.For<ILancamentoDao>();
        }

        [Fact]
        public async Task Realizar_Lancamento_Credito()
        {
            var message = new CreditoMessage
            {
                DataHora = new DateTime(2024, 8, 12),
                EventId = Guid.NewGuid(),
                LancamentoId = Guid.NewGuid(),
                Valor = 10,
                Descricao = "Descricao",
                TimeStamp = DateTime.Now,
            };

            var creditoService = new CreditoService(_dao);
            await creditoService.Lancar(message);
            await _dao.Received().Gravar(Arg.Any<Lancamento>());
        }



        [Fact]
        public async Task Realizar_Lancamento_Debito()
        {
            var message = new DebitoMessage
            {
                DataHora = new DateTime(2024, 8, 12),
                EventId = Guid.NewGuid(),
                LancamentoId = Guid.NewGuid(),
                Valor = 10,
                Descricao = "Descricao",
                TimeStamp = DateTime.Now,
            };

            var debitoResult = new Debito(message.LancamentoId, message.DataHora, message.Valor, message.Descricao);
            var debitoService = new DebitoService(_dao);
            await debitoService.Lancar(message);
            await _dao.Received().Gravar(Arg.Any<Lancamento>());
        }
    }
}