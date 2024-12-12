using MediatR;

namespace ControleLancamento.Application.Commands
{
    public abstract class EfetuarLancamentoCommand : IRequest
    {
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
    }
}
