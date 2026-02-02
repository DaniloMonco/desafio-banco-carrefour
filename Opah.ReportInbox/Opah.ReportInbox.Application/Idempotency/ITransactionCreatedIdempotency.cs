using Opah.ReportInbox.Application.Consumer.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Application.Idempotency
{
    public interface ITransactionCreatedIdempotency
    {
        Task VerifyAlreadyProcessed(TransactionCreatedMessage transaction);
    }
}
