using NSubstitute;
using Opah.TransactionOutbox.Application.Publisher;
using Opah.TransactionOutbox.Application.Publisher.Message;
using Opah.TransactionOutbox.Application.Services;
using Opah.TransactionOutbox.Domain.Entities;
using Opah.TransactionOutbox.Domain.Events;
using Opah.TransactionOutbox.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionOutbox.Application.Test
{
    public class TransactionCreatedServiceTests
    {
        private readonly ITransactionEventPublisher<TransactionCreateMessage> _publisher;
        private readonly ITransactionOutboxRepository _repository;
        private readonly TransactionCreatedService _service;

        public TransactionCreatedServiceTests()
        {
            _publisher = Substitute.For<ITransactionEventPublisher<TransactionCreateMessage>>();
            _repository = Substitute.For<ITransactionOutboxRepository>();

            _service = new TransactionCreatedService(
                _publisher,
                _repository
            );
        }

        [Fact]
        public async Task Execute_Should_Do_Nothing_When_No_Transactions_Found()
        {
            // Arrange
            _repository.GetTransactionsCreated()
                       .Returns(new List<TransactionCreated>());

            // Act
            await _service.Execute();

            // Assert
            await _publisher.DidNotReceive()
                .Publish(Arg.Any<TransactionCreateMessage>());

            await _repository.DidNotReceive()
                .Processed(Arg.Any<TransactionCreated>());
        }

        [Fact]
        public async Task Execute_Should_Publish_And_Mark_As_Processed_For_Each_Transaction()
        {
            // Arrange
            var transactionEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                100m,
                TransactionType.Credit,
                DateTime.UtcNow,
                "user"
            );

            var transaction = new TransactionCreated
            {
                Id = Guid.NewGuid(),
                Payload = Newtonsoft.Json.JsonConvert.SerializeObject(transactionEvent),
                ProcessedOnUtc = default
            };

            _repository.GetTransactionsCreated()
                       .Returns(new List<TransactionCreated> { transaction });

            // Act
            await _service.Execute();

            // Assert
            await _publisher.Received(1)
                .Publish(Arg.Any<TransactionCreateMessage>());

            await _repository.Received(1)
                .Processed(transaction);

            Assert.NotEqual(default, transaction.ProcessedOnUtc);
        }

        [Fact]
        public async Task Execute_Should_Process_Multiple_Transactions()
        {
            // Arrange
            var transactions = new List<TransactionCreated>
            {
                CreateTransaction(),
                CreateTransaction()
            };

            _repository.GetTransactionsCreated()
                       .Returns(transactions);

            // Act
            await _service.Execute();

            // Assert
            await _publisher.Received(2)
                .Publish(Arg.Any<TransactionCreateMessage>());

            foreach (var transaction in transactions)
            {
                await _repository.Received(1)
                    .Processed(transaction);

                Assert.NotEqual(default, transaction.ProcessedOnUtc);
            }
        }

        private static TransactionCreated CreateTransaction()
        {
            var ev = new TransactionCreatedEvent(
                Guid.NewGuid(),
                50m,
                TransactionType.Debit,
                DateTime.UtcNow,
                null
            );

            return new TransactionCreated
            {
                Id = Guid.NewGuid(),
                Payload = Newtonsoft.Json.JsonConvert.SerializeObject(ev)
            };
        }
    }
}
