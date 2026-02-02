using NSubstitute;
using Opah.TransactionService.Application.Dtos;
using Opah.TransactionService.Application.Persistence;
using Opah.TransactionService.Domain.Repositories;
using Opah.TransactionService.Domain.ValueObject;
using System.Transactions;

namespace Opah.TransactionService.Application.Test
{
    public class TransactionServiceTests
    {
        private readonly IUnitOfWork _uow;
        private readonly ITransactionCreatedOutboxRepository _outboxRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly Services.TransactionService _service;

        public TransactionServiceTests()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _outboxRepository = Substitute.For<ITransactionCreatedOutboxRepository>();
            _transactionRepository = Substitute.For<ITransactionRepository>();

            _service = new Services.TransactionService(
                _uow,
                _outboxRepository,
                _transactionRepository
            );
        }

        [Fact]
        public async Task Create_Should_Create_Transaction_And_Commit()
        {
            // Arrange
            var dto = new TransactionCreditDto(100m);

            // Act
            var result = await _service.Create(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Amount, result.Amount);
            Assert.Equal(TransactionType.Credit, result.Type);

            _uow.Received(1).BeginTransaction();
            await _transactionRepository.Received(1).Save(Arg.Any<Domain.Entities.Transaction>());
            await _outboxRepository.Received(1).Save(Arg.Any<Queue<Domain.Events.TransactionEvent>>());
            _uow.Received(1).Commit();
            _uow.DidNotReceive().Rollback();
        }

        [Fact]
        public async Task Create_Should_Rollback_When_TransactionRepository_Fails()
        {
            // Arrange
            var dto = new TransactionDebitDto(100m);

            _transactionRepository
                .Save(Arg.Any<Domain.Entities.Transaction>())
                .Returns<Task>(_ => throw new Exception("db error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _service.Create(dto)
            );

            // Assert
            Assert.Equal("db error", exception.Message);

            _uow.Received(1).BeginTransaction();
            _uow.Received(1).Rollback();
            _uow.DidNotReceive().Commit();
        }

        [Fact]
        public async Task Create_Should_Rollback_When_OutboxRepository_Fails()
        {
            // Arrange
            var dto = new TransactionCreditDto(200m);

            _outboxRepository
                .Save(Arg.Any<Queue<Domain.Events.TransactionEvent>>())
                .Returns<Task>(_ => throw new Exception("outbox error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _service.Create(dto)
            );

            // Assert
            Assert.Equal("outbox error", exception.Message);

            _uow.Received(1).BeginTransaction();
            _uow.Received(1).Rollback();
            _uow.DidNotReceive().Commit();
        }

        [Fact]
        public async Task Create_Should_Call_Repositories_In_Order()
        {
            // Arrange
            var dto = new TransactionCreditDto(300m);

            // Act
            await _service.Create(dto);

            // Assert (ordem importa!)
            Received.InOrder(() =>
            {
                _uow.BeginTransaction();
                _transactionRepository.Save(Arg.Any<Domain.Entities.Transaction>());
                _outboxRepository.Save(Arg.Any<Queue<Domain.Events.TransactionEvent>>());
                _uow.Commit();
            });
        }
    }
}
