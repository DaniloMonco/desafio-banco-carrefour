using Opah.TransactionOutbox.Application.Mapper;
using Opah.TransactionOutbox.Application.Publisher.Message;
using Opah.TransactionOutbox.Domain.Events;

namespace Opah.TransactionOutbox.Application.Test
{
    public class TransactionCreateMapperTests
    {
        [Fact]
        public void ToMessage_Should_Map_All_Fields_Correctly()
        {
            // Arrange
            var domainEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                150.50m,
                TransactionType.Credit,
                new DateTime(2025, 1, 10, 12, 0, 0),
                "danilo"
            );

            // Act
            var message = domainEvent.ToMessage();

            // Assert
            Assert.NotNull(message);
            Assert.Equal(domainEvent.TransactionId, message.TransactionId);
            Assert.Equal(domainEvent.Amount, message.Amount);
            Assert.Equal(
                TransactionMessageType.Credit,
                message.TransactionType);
            Assert.Equal(domainEvent.OccurredAt, message.OccurredAt);
            Assert.Equal(domainEvent.UserName, message.UserName);
        }

        [Fact]
        public void ToMessage_Should_Map_Debit_Transaction_Correctly()
        {
            // Arrange
            var domainEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                100m,
                TransactionType.Debit,
                DateTime.UtcNow,
                null
            );

            // Act
            var message = domainEvent.ToMessage();

            // Assert
            Assert.Equal(TransactionMessageType.Debit, message.TransactionType);
            Assert.Null(message.UserName);
        }

        [Fact]
        public void ToMessage_Should_Create_New_Instance()
        {
            // Arrange
            var domainEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                10m,
                TransactionType.Credit,
                DateTime.UtcNow,
                "user"
            );

            // Act
            var message1 = domainEvent.ToMessage();
            var message2 = domainEvent.ToMessage();

            // Assert
            Assert.NotSame(message1, message2);
            Assert.Equal(message1, message2);
        }
    }
}
