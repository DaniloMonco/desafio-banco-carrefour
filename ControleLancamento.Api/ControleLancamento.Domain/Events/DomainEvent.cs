using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Domain.Events
{
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            TimeStamp = DateTime.Now;
        }
        public Guid EventId { get; private set; }
        public DateTime TimeStamp { get; private set; }
    }
}
