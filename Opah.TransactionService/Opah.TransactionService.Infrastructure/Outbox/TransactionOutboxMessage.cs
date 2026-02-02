using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionService.Infrastructure.Outbox
{
    internal record TransactionOutbox(string Payload)
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; protected set; } = DateTime.UtcNow;
    }
}
