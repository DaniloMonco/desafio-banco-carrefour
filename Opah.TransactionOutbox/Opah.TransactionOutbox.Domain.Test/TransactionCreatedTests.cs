using Newtonsoft.Json;
using Opah.TransactionOutbox.Domain.Entities;
using Opah.TransactionOutbox.Domain.Events;

namespace Opah.TransactionOutbox.Domain.Test
{
    public class TransactionCreatedTests
    {
        [Fact]
        public void GetEvent_Should_Deserialize_Payload_Correctly()
        {
            // Arrange
            var expectedEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                150.75m,
                TransactionType.Credit,
                new DateTime(2025, 1, 10, 14, 30, 0),
                "danilo"
            );

            var payload = JsonConvert.SerializeObject(expectedEvent);

            var entity = new TransactionCreated
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                ProcessedOnUtc = DateTime.UtcNow,
                Payload = payload
            };

            // Act
            var result = entity.GetEvent();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEvent.TransactionId, result.TransactionId);
            Assert.Equal(expectedEvent.Amount, result.Amount);
            Assert.Equal(expectedEvent.TransactionType, result.TransactionType);
            Assert.Equal(expectedEvent.OccurredAt, result.OccurredAt);
            Assert.Equal(expectedEvent.UserName, result.UserName);
        }

        [Fact]
        public void GetEvent_Should_Return_Equivalent_Record()
        {
            // Arrange
            var expectedEvent = new TransactionCreatedEvent(
                Guid.NewGuid(),
                100m,
                TransactionType.Debit,
                DateTime.UtcNow,
                null
            );

            var entity = new TransactionCreated
            {
                Payload = JsonConvert.SerializeObject(expectedEvent)
            };

            // Act
            var result = entity.GetEvent();

            // Assert
            Assert.Equal(expectedEvent, result);
        }

        [Fact]
        public void GetEvent_Should_Throw_When_Payload_Is_Invalid_Json()
        {
            // Arrange
            var entity = new TransactionCreated
            {
                Payload = "{ invalid json"
            };

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => entity.GetEvent());
        }

        [Fact]
        public void GetEvent_Should_Null_When_Payload_Is_Empty()
        {
            // Arrange
            var entity = new TransactionCreated
            {
                Payload = string.Empty
            };

            // Act
            var result = entity.GetEvent();
            // Assert
            TransactionCreatedEvent expectedEvent = null;
            Assert.Equal(expectedEvent, result);

        }
    }
}
