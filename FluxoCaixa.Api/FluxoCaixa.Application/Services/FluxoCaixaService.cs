using FluxoCaixa.Application.Dto;
using FluxoCaixa.Application.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Services
{
    public class FluxoCaixaService : IFluxoCaixaService
    {
        private readonly Domain.DomainService.IFluxoCaixaService _domainService;
        public FluxoCaixaService(Domain.DomainService.IFluxoCaixaService domainService)
        {
            _domainService = domainService;
        }

        public async Task<IEnumerable<FluxoCaixaDto>> RecuperarFluxoCaixa(int ano, int mes, int dia)
        {
            var model = await _domainService.RecuperarFluxoCaixa(ano, mes, dia);
            return model.ToDto();
        }

        public async Task<IEnumerable<FluxoCaixaDto>> RecuperarFluxoCaixa(int ano, int mes)
        {
            var model = await _domainService.RecuperarFluxoCaixa(ano, mes);

            return model.ToDto();
        }
    }
}
