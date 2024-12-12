using FluxoCaixa.Application.Dto;

namespace FluxoCaixa.Application.Services
{
    public interface IFluxoCaixaService 
    {
        Task<IEnumerable<FluxoCaixaDto>> RecuperarFluxoCaixa(int ano, int mes, int dia);
        Task<IEnumerable<FluxoCaixaDto>> RecuperarFluxoCaixa(int ano, int mes);
    }
}
