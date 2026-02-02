using Opah.TransactionService.Domain.Events;
using Opah.TransactionService.Domain.ValueObject;

namespace Opah.Transaction.Domain.Test
{
    public class TransactionTests
    {
        [Fact]
        public void Create_Should_Create_Transaction_With_Valid_Data()
        {
            // Arrange
            var transaction = new Opah.TransactionService.Domain.Entities.Transaction();
            var amount = 100m;
            var type = TransactionType.Credit;
            var userName = "danilo";

            // Act
            transaction.Create(amount, type, userName);

            // Assert
            Assert.NotEqual(Guid.Empty, transaction.Id);
            Assert.Equal(amount, transaction.Amount);
            Assert.Equal(type, transaction.Type);
            Assert.True(transaction.OccurredAt <= DateTime.Now);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Create_Should_Throw_Exception_When_Amount_Is_Invalid(decimal amount)
        {
            // Arrange
            var transaction = new Opah.TransactionService.Domain.Entities.Transaction();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                transaction.Create(amount, TransactionType.Debit, "user")
            );

            Assert.Equal("Amount must be positive (Parameter 'amount')", exception.Message);
            Assert.Equal("amount", exception.ParamName);
        }

        [Fact]
        public void Create_Should_Add_TransactionCreated_Event()
        {
            // Arrange
            var transaction = new Opah.TransactionService.Domain.Entities.Transaction();

            // Act
            transaction.Create(200m, TransactionType.Debit, "user");

            // Assert
            var events = transaction.GetEvents();

            Assert.Single(events);
            Assert.IsType<TransactionCreated>(events.First());
        }

        [Fact]
        public void Create_With_OccurredAt_Should_Use_Provided_Date()
        {
            // Arrange
            var transaction = new Opah.TransactionService.Domain.Entities.Transaction();
            var occurredAt = new DateTime(2025, 1, 1);

            // Act
            transaction.Create(300m, TransactionType.Credit, occurredAt, "user");

            // Assert
            Assert.Equal(occurredAt, transaction.OccurredAt);
        }

        [Fact]
        public void TransactionCreated_Event_Should_Contain_Correct_Data()
        {
            // Arrange
            var transaction = new Opah.TransactionService.Domain.Entities.Transaction();
            var amount = 150m;
            var type = TransactionType.Credit;
            var userName = "danilo";
            var occurredAt = DateTime.UtcNow;

            // Act
            transaction.Create(amount, type, occurredAt, userName);
            var @event = transaction.GetEvents().First();

            // Assert
            Assert.Equal(transaction.Id, @event.TransactionId);
            Assert.Equal(amount, @event.Amount);
            Assert.Equal(type, @event.TransactionType);
            Assert.Equal(occurredAt, @event.OccurredAt);
            Assert.Equal(userName, @event.UserName);
        }
    }
}
