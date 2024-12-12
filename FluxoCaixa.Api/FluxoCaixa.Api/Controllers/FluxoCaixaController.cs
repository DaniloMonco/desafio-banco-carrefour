using FluxoCaixa.Application.Dto;
using FluxoCaixa.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FluxoCaixaController : ControllerBase
    {
        private readonly ILogger<FluxoCaixaController> _logger;
        private readonly IFluxoCaixaService _service;

        public FluxoCaixaController(ILogger<FluxoCaixaController> logger, IFluxoCaixaService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("Diario")]
        public async Task<IActionResult> FluxoDiarioDiario(int ano, int mes, int dia)
        {
            return Ok(await _service.RecuperarFluxoCaixa(ano, mes, dia));
        }

        [HttpGet]
        [Route("Mensal")]
        public async Task<IActionResult> FluxoDiarioMensal(int ano, int mes)
        {
            return Ok(await _service.RecuperarFluxoCaixa(ano, mes));
        }
    }
}
