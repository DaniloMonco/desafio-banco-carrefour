using Opah.TransactionService.Domain.Events;
using Opah.TransactionService.Domain.ValueObject;

namespace Opah.TransactionService.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public DateTime OccurredAt { get; private set; }

        private Queue<TransactionEvent> _events = new();


        public void Create(decimal amount, TransactionType type, string? userName)
            => Create(amount, type, DateTime.Now, userName);

        public void Create(decimal amount, TransactionType type, DateTime occurredAt, string? userName)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            Id = Guid.NewGuid();
            Amount = amount;
            Type = type;
            OccurredAt = occurredAt;

            _events.Enqueue(new TransactionCreated(
                Id,
                amount,
                type,
                OccurredAt,
                userName
            ));
        }

        public Queue<TransactionEvent> GetEvents() => _events;
    }

}
