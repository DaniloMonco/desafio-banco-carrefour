using FluxoCaixa.Domain.Model;

namespace FluxoCaixa.Domain.DomainService
{
    public interface IFluxoCaixaService
    {
        Task<Model.FluxoCaixa> RecuperarFluxoCaixa(int ano, int mes, int dia);

        Task<Model.FluxoCaixa> RecuperarFluxoCaixa(int ano, int mes);
    }
}
