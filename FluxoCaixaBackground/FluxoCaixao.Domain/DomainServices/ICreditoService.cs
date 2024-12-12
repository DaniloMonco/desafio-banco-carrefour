using FluxoCaixa.Application.Messages;

namespace FluxoCaixa.Domain.DomainServices
{
    public interface ICreditoService
    {
        Task Lancar(CreditoMessage message);
    }
}
