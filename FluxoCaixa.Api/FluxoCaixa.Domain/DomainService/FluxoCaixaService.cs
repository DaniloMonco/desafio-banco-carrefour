using FluxoCaixa.Domain.Cached;
using FluxoCaixa.Domain.Model;
using FluxoCaixa.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FluxoCaixa.Domain.DomainService
{
    public class FluxoCaixaService : IFluxoCaixaService
    {
        private readonly ILancamentoRepository _repository;
        private readonly IFluxoCaixaCached _cache;
        public FluxoCaixaService(ILancamentoRepository repository, IFluxoCaixaCached cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Model.FluxoCaixa> RecuperarFluxoCaixa(int ano, int mes, int dia)
        {
            var cachekey = $"RecuperarFluxoCaixa-{ano}-{mes}-{dia}";
            var fluxoCaixaCache = await _cache.GetAsync<Model.FluxoCaixa>(cachekey);
            if (fluxoCaixaCache is null)
            {
                var lancamentos = await _repository.RecuperarLancamentos(ano, mes, dia);
                var fluxoCaixa= MontarFluxoCaixa(ano, mes, lancamentos);
                await _cache.SetAsync<Model.FluxoCaixa>(cachekey, fluxoCaixa, TimeSpan.FromMinutes(5));
                return fluxoCaixa;
            }

            return fluxoCaixaCache;
        }

        public async Task<Model.FluxoCaixa> RecuperarFluxoCaixa(int ano, int mes)
        {
            var cachekey = $"RecuperarFluxoCaixa-{ano}-{mes}";
            var fluxoCaixaCache = await _cache.GetAsync<Model.FluxoCaixa>(cachekey);
            if (fluxoCaixaCache is null)
            {
                var lancamentos = await _repository.RecuperarLancamentos(ano, mes);
                var fluxoCaixa = MontarFluxoCaixa(ano, mes, lancamentos);
                await _cache.SetAsync<Model.FluxoCaixa>(cachekey, fluxoCaixa, TimeSpan.FromHours(12));
                return fluxoCaixa;
            }

            return fluxoCaixaCache;
        }

        private Model.FluxoCaixa MontarFluxoCaixa(int ano, int mes, IEnumerable<Lancamento> lancamentos)
        {
            var fluxoCaixa = Model.FluxoCaixa.Criar(ano, mes);
            fluxoCaixa.MontarFluxoCaixa(lancamentos);
            return fluxoCaixa;
        }
    }
}
