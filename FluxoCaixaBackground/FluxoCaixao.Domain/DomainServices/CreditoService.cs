using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.DAO;
using FluxoCaixa.Domain.Mappers;

namespace FluxoCaixa.Domain.DomainServices
{
    public class CreditoService : ICreditoService
    {
        private readonly ILancamentoDao _dao;
        public CreditoService(ILancamentoDao dao)
        {
            _dao = dao;
        }

        public async Task Lancar(CreditoMessage message)
        {
            var entity = message.ToModel();
            await _dao.Gravar(entity);
        }
    }
}
