using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.DAO;
using FluxoCaixa.Domain.Mappers;

namespace FluxoCaixa.Domain.DomainServices
{
    public class DebitoService : IDebitoService
    {
        private readonly ILancamentoDao _dao;
        public DebitoService(ILancamentoDao dao)
        {
            _dao = dao;
        }

        public async Task Lancar(DebitoMessage message)
        {
            var entity = message.ToModel();
            await _dao.Gravar(entity);
        }
    }
}
