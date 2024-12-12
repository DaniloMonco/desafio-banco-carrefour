using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.Model;

namespace FluxoCaixa.Domain.Mappers
{
    public static class LancamentoMapper
    {
        public static Debito ToModel(this DebitoMessage message)
            => new Debito(message.LancamentoId, message.DataHora, message.Valor, message.Descricao);

        public static Credito ToModel(this CreditoMessage message)
            => new Credito(message.LancamentoId, message.DataHora, message.Valor, message.Descricao);
    }
}
