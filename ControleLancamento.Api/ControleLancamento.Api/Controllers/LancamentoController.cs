using ControleLancamento.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleLancamento.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LancamentoController : ControllerBase
    {
        private readonly ILogger<LancamentoController> _logger;
        private readonly IMediator _mediator;
        public LancamentoController(ILogger<LancamentoController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Debito")]
        public async Task<IActionResult> Debito(LancarDebitoCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("Credito")]
        public async Task<IActionResult> Credito(LancarCreditoCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
