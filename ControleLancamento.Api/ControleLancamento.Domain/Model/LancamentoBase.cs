using ControleLancamento.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Domain.Model
{
    public abstract class LancamentoBase : IAggregate
    {
        protected readonly Queue<DomainEvent> _domainEvents;
        protected LancamentoBase(DateTime dataHora, decimal valor, string descricao)
        {
            
            Id = Guid.NewGuid();
            _domainEvents = new Queue<DomainEvent>();
            DataHora = dataHora;
            Valor = valor;
            Descricao = descricao;
            TipoLancamento = DefinirTipoLancamento();
        }

        public Guid Id { get; private set; }
        public DateTime DataHora { get; protected set;}
        public decimal Valor { get; protected set;}
        public string Descricao { get; protected set;}
        public TipoLancamento TipoLancamento { get; private set;}

        protected abstract TipoLancamento DefinirTipoLancamento();
        protected void AdicionarEvento(DomainEvent @event)
        {
            _domainEvents.Enqueue(@event);
        }

        public IEnumerable<DomainEvent> RecuperarEventos()
        {
            return _domainEvents.ToArray();
        }
    }
}
