using Opah.TransactionService.Application.Dtos;
using Opah.TransactionService.Application.Persistence;
using Opah.TransactionService.Domain.Entities;
using Opah.TransactionService.Domain.Repositories;
using Opah.TransactionService.Domain.ValueObject;

namespace Opah.TransactionService.Application.Services
{
    public sealed class TransactionService(IUnitOfWork uow, ITransactionCreatedOutboxRepository outboxRepository, ITransactionRepository transactionRepository)
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly ITransactionCreatedOutboxRepository _outboxRepository = outboxRepository; 
        private readonly ITransactionRepository _transactionRepository = transactionRepository;

        public async Task<Transaction> Create(TransactionDto dto)
        {
            var transaction = new Transaction();
            transaction.Create(dto.Amount, (TransactionType)dto.Type, dto.UserName);

            try
            {
                _uow.BeginTransaction();
                await _transactionRepository.Save(transaction);
                await _outboxRepository.Save(transaction.GetEvents());
                _uow.Commit();
            }
            catch (Exception)
            {
                _uow.Rollback();
                throw;
            }
            
            return transaction;
        }
    }
}
