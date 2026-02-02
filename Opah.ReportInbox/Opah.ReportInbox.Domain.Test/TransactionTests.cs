using Opah.ReportInbox.Domain.Entities;
using Opah.ReportInbox.Domain.ValueObjects;

namespace Opah.ReportInbox.Domain.Test
{
    public class TransactionTests
    {
        [Fact]
        public void Should_Create_Transaction_With_Provided_Values()
        {
            // Arrange
            var id = Guid.NewGuid();
            var occurredAt = DateTime.UtcNow;
            var value = 150.75m;
            var description = "Payment received";
            var type = TransactionType.Credit;

            // Act
            var transaction = new Transaction(
                id,
                occurredAt,
                value,
                description,
                type
            );

            // Assert
            Assert.Equal(id, transaction.Id);
            Assert.Equal(occurredAt, transaction.OccurredAt);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(description, transaction.Description);
            Assert.Equal(type, transaction.TransactionType);
        }

        [Fact]
        public void Records_With_Same_Values_Should_Be_Equal()
        {
            // Arrange
            var id = Guid.NewGuid();
            var occurredAt = new DateTime(2025, 1, 10);
            var value = 100m;
            var description = "Test";
            var type = TransactionType.Debit;

            var transaction1 = new Transaction(id, occurredAt, value, description, type);
            var transaction2 = new Transaction(id, occurredAt, value, description, type);

            // Assert
            Assert.Equal(transaction1, transaction2);
            Assert.True(transaction1 == transaction2);
        }

        [Fact]
        public void Records_With_Different_Values_Should_Not_Be_Equal()
        {
            // Arrange
            var transaction1 = new Transaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                100m,
                "Transaction 1",
                TransactionType.Credit
            );

            var transaction2 = new Transaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                200m,
                "Transaction 2",
                TransactionType.Debit
            );

            // Assert
            Assert.NotEqual(transaction1, transaction2);
            Assert.True(transaction1 != transaction2);
        }

        [Fact]
        public void With_Expression_Should_Create_New_Instance_With_Updated_Value()
        {
            // Arrange
            var original = new Transaction(
                Guid.NewGuid(),
                DateTime.UtcNow,
                100m,
                "Original",
                TransactionType.Credit
            );

            // Act
            var updated = original with { Value = 200m };

            // Assert
            Assert.NotSame(original, updated);
            Assert.Equal(100m, original.Value);
            Assert.Equal(200m, updated.Value);
        }

    }
}
