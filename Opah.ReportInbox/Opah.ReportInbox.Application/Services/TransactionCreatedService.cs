using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Application.Idempotency;
using Opah.ReportInbox.Application.Mapper;
using Opah.ReportInbox.Domain.Repositories;

namespace Opah.ReportInbox.Application.Services
{
    public class TransactionCreatedService(IUnitOfWork uow, ITransactionCreatedIdempotency idempotency, ITransactionRepository repository)
    {
        protected readonly IUnitOfWork _uow = uow;
        protected readonly ITransactionCreatedIdempotency _idempotency = idempotency;
        protected readonly ITransactionRepository _repository = repository;

        public async Task Process(TransactionCreatedMessage transaction)
        {
            try
            {
                _uow.BeginTransaction();
                await _idempotency.VerifyAlreadyProcessed(transaction);
                await _repository.Save(transaction.ToEntity());
                _uow.Commit();
            }
            catch(Exception)
            {
                _uow.Rollback();
                throw;
            }
        }
    }
}
