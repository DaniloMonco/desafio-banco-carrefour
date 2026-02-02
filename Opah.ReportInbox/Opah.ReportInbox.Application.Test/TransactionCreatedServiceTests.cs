using NSubstitute;
using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Application.Idempotency;
using Opah.ReportInbox.Application.Services;
using Opah.ReportInbox.Domain.Repositories;
using Opah.ReportInbox.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Application.Test
{
    public class TransactionCreatedServiceTests
    {
        private readonly IUnitOfWork _uow;
        private readonly ITransactionCreatedIdempotency _idempotency;
        private readonly ITransactionRepository _repository;
        private readonly TransactionCreatedService _service;

        public TransactionCreatedServiceTests()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _idempotency = Substitute.For<ITransactionCreatedIdempotency>();
            _repository = Substitute.For<ITransactionRepository>();

            _service = new TransactionCreatedService(
                _uow,
                _idempotency,
                _repository
            );
        }

        [Fact]
        public async Task Process_Should_Commit_When_All_Steps_Succeed()
        {
            // Arrange
            var message = new TransactionCreatedMessage(
                Guid.NewGuid(),
                100m,
                TransactionTypeMessage.Credit,
                DateTime.UtcNow);

            _idempotency
                .VerifyAlreadyProcessed(message)
                .Returns(Task.CompletedTask);

            // Act
            await _service.Process(message);

            // Assert
            Received.InOrder(() =>
            {
                _uow.BeginTransaction();
                _idempotency.VerifyAlreadyProcessed(message);
                _repository.Save(Arg.Any<Domain.Entities.Transaction>());
                _uow.Commit();
            });

            _uow.DidNotReceive().Rollback();
        }

        [Fact]
        public async Task Process_Should_Rollback_And_Rethrow_When_Idempotency_Fails()
        {
            // Arrange
            var message = new TransactionCreatedMessage
            (
                Guid.NewGuid(),
                100m,
                TransactionTypeMessage.Credit,
                DateTime.UtcNow
            );

            _idempotency
                .VerifyAlreadyProcessed(message)
                .Returns<Task>(_ => throw new InvalidOperationException("Duplicated"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.Process(message)
            );

            _uow.Received(1).BeginTransaction();
            _uow.Received(1).Rollback();
            _uow.DidNotReceive().Commit();

            await _repository.DidNotReceive()
                .Save(Arg.Any<Domain.Entities.Transaction>());
        }

        [Fact]
        public async Task Process_Should_Rollback_And_Rethrow_When_Save_Fails()
        {
            // Arrange
            var message = new TransactionCreatedMessage
            (
                Guid.NewGuid(),
                100m, 
                TransactionTypeMessage.Credit,
                DateTime.UtcNow
             );

            _repository
                .Save(Arg.Any<Domain.Entities.Transaction>())
                .Returns<Task>(_ => throw new Exception("DB error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _service.Process(message)
            );

            _uow.Received(1).BeginTransaction();
            _uow.Received(1).Rollback();
            _uow.DidNotReceive().Commit();
        }
    }
}
