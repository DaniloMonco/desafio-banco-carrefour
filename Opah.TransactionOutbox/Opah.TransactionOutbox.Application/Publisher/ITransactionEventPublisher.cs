using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionOutbox.Application.Publisher
{
    public interface ITransactionEventPublisher<T>
    {
        Task Publish(T message);
    }
}
