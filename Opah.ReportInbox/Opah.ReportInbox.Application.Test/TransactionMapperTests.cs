using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Application.Mapper;
using Opah.ReportInbox.Domain.ValueObjects;

namespace Opah.ReportInbox.Application.Test
{
    public class TransactionMapperTests
    {
        [Fact]
        public void ToEntity_Should_Map_All_Fields_Correctly()
        {
            // Arrange
            var message = new TransactionCreatedMessage(Guid.NewGuid(), 150.75m, TransactionTypeMessage.Credit, new DateTime(2025, 1, 10, 14, 30, 0));

            // Act
            var entity = message.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(message.TransactionId, entity.Id);
            Assert.Equal(message.OccurredAt, entity.OccurredAt);
            Assert.Equal(message.Amount, entity.Value);
            Assert.Equal(string.Empty, entity.Description);
            Assert.Equal(TransactionType.Credit, entity.TransactionType);
        }

        [Fact]
        public void ToEntity_Should_Map_Debit_Transaction_Correctly()
        {
            // Arrange
            var message = new TransactionCreatedMessage(Guid.NewGuid(), 50m, TransactionTypeMessage.Debit, DateTime.UtcNow);

            // Act
            var entity = message.ToEntity();

            // Assert
            Assert.Equal(TransactionType.Debit, entity.TransactionType);
            Assert.Equal(50m, entity.Value);
        }
    }
}
