using Opah.TransactionService.Domain.ValueObject;

namespace Opah.TransactionService.Domain.Events
{
    public abstract class TransactionEvent
    {
        protected TransactionEvent(Guid transactionId, decimal amount, TransactionType transactionType, DateTime occurredAt, string? userName)
        {
            TransactionId = transactionId;
            Amount = amount;
            TransactionType = transactionType;
            OccurredAt = occurredAt;
            UserName = userName;
        }
        public Guid TransactionId { get; protected set; }
        public decimal Amount { get; protected set; }
        public TransactionType TransactionType { get; protected set; }
        public DateTime OccurredAt { get; protected set; }
        public string? UserName { get; protected set; }
    }
}
