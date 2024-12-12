using FluxoCaixa.Application.Messages;

namespace FluxoCaixa.Domain.DomainServices
{
    public interface IDebitoService
    {
        Task Lancar(DebitoMessage message);
    }
}
