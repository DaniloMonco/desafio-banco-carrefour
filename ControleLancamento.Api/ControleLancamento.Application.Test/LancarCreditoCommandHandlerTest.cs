using ControleLancamento.Application.Commands;
using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Model;
using ControleLancamento.Domain.Repository;
using NSubstitute;

namespace ControleLancamento.Application.Test
{
    public class LancarCreditoCommandHandlerTest
    {
        private ICreditoLancadoPublisher _creditoEventBus;
        private IDebitoLancadoPublisher _debitoEventBus;
        private ILancamentoRepository _repository;

        public LancarCreditoCommandHandlerTest()
        {
            _creditoEventBus = Substitute.For<ICreditoLancadoPublisher>();
            _debitoEventBus = Substitute.For<IDebitoLancadoPublisher>();
            _repository = Substitute.For<ILancamentoRepository>();
        }


        [Fact]
        public async Task Executar_CreditoCommandHandler_Quando_CreditoCommand_Executado()
        {
            var command = new LancarCreditoCommand { DataHora = DateTime.Now, Valor = 10, Descricao = "Descricao" };
            var handler = new LancarCreditoCommandHandler(_creditoEventBus, _repository);
            await handler.Handle(command, CancellationToken.None);

            await _creditoEventBus.Received().Publicar(Arg.Any<Credito>());
            await _repository.Received().Salvar(Arg.Any<Credito>());
        }

        [Fact]
        public async Task Executar_DebitoCommandHandler_Quando_DebitoCommand_Executado()
        {
            var command = new LancarDebitoCommand { DataHora = DateTime.Now, Valor = 10, Descricao = "Descricao" };
            var handler = new LancarDebitoCommandHandler(_debitoEventBus, _repository);
            await handler.Handle(command, CancellationToken.None);

            await _debitoEventBus.Received().Publicar(Arg.Any<Debito>());
            await _repository.Received().Salvar(Arg.Any<Debito>());
        }
    }
}