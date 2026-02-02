using Opah.TransactionService.Domain.Events;

namespace Opah.TransactionService.Domain.Repositories
{
    public interface ITransactionCreatedOutboxRepository
    {
        Task Save(IEnumerable<TransactionEvent> transaction);

    }
}
