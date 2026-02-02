using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Channels;
using Opah.TransactionOutbox.Application.Publisher;
using Opah.TransactionOutbox.Application.Publisher.Message;

namespace Opah.TransactionOutbox.Infrastructure.RabbitMQ
{

    public class TransactionCreatedPublisher(RabbitMQContext context) : TransactionPublisher<TransactionCreateMessage>(context), ITransactionEventPublisher<TransactionCreateMessage>
    {
        protected override string SetExchangeName()
            => "transaction.created";
    }
}
