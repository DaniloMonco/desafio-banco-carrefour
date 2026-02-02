using System;
using System.Collections.Generic;
using System.Text;
using Opah.TransactionOutbox.Application.Mapper;
using Opah.TransactionOutbox.Application.Publisher;
using Opah.TransactionOutbox.Application.Publisher.Message;
using Opah.TransactionOutbox.Domain.Entities;
using Opah.TransactionOutbox.Domain.Repositories;

namespace Opah.TransactionOutbox.Application.Services
{
    public sealed class TransactionCreatedService(ITransactionEventPublisher<TransactionCreateMessage> eventPublisher, ITransactionOutboxRepository repository)
    {
        private readonly ITransactionEventPublisher<TransactionCreateMessage> _eventPublisher = eventPublisher;
        private readonly ITransactionOutboxRepository _repository = repository;

        public async Task Execute()
        {
            var transactions = await _repository.GetTransactionsCreated();
            if (!transactions.Any())
            {
                await Task.Delay(500);
                return;
            }

            foreach (TransactionCreated transaction in transactions)
            {
                var transactionEvent = transaction.GetEvent();

                var transactionCreateMessage = transactionEvent.ToMessage();

                await _eventPublisher.Publish(transactionCreateMessage);
                
                transaction.ProcessedOnUtc = DateTime.UtcNow;
                await _repository.Processed(transaction);
            }
        }
    }
}
