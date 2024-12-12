using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Model;
using ControleLancamento.Domain.Repository;
using MediatR;

namespace ControleLancamento.Application.Commands
{
    public class LancarCreditoCommandHandler : IRequestHandler<LancarCreditoCommand>
    {
        private readonly ICreditoLancadoPublisher _eventBus;
        private readonly ILancamentoRepository _repository;
        public LancarCreditoCommandHandler(ICreditoLancadoPublisher eventBus, ILancamentoRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task Handle(LancarCreditoCommand request, CancellationToken cancellationToken)
        {
            var credito = Credito.Lancar(request.DataHora, request.Valor, request.Descricao);
            await _repository.Salvar(credito);
            await _eventBus.Publicar(credito);
        }
    }
}
