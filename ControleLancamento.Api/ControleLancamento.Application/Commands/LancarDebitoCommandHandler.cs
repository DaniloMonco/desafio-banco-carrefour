using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Model;
using ControleLancamento.Domain.Repository;
using MediatR;

namespace ControleLancamento.Application.Commands
{
    public class LancarDebitoCommandHandler : IRequestHandler<LancarDebitoCommand>
    {
        private readonly IDebitoLancadoPublisher _eventBus;
        private readonly ILancamentoRepository _repository;
        public LancarDebitoCommandHandler(IDebitoLancadoPublisher eventBus, ILancamentoRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task Handle(LancarDebitoCommand request, CancellationToken cancellationToken)
        {
            var debito = Debito.Lancar(request.DataHora, request.Valor, request.Descricao);
            await _repository.Salvar(debito);
            await _eventBus.Publicar(debito);
        }
    }
}
