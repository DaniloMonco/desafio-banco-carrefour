using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Opah.TransactionOutbox.Domain.Events;

namespace Opah.TransactionOutbox.Domain.Entities
{
    public class TransactionCreated
    {
        public Guid Id { get; set; }
        public DateTime OccurredOnUtc { get; set;  }
        public DateTime ProcessedOnUtc { get; set;  }
        public string Payload { get; set; }

        public TransactionCreatedEvent GetEvent()
            => JsonConvert.DeserializeObject<TransactionCreatedEvent>(Payload)!;
    }
}
