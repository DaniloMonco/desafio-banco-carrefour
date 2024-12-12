using ControleLancamento.Domain.Model;
using ControleLancamento.Domain.Repository;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ControleLancamento.Infrastructure.Repository
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly EventStoreClient _eventStore;

        public LancamentoRepository(EventStoreClient eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task Salvar(Debito debito)
        {
            var streamName = $"FluxoCaixaStream-{debito.DataHora.Year}-{debito.DataHora.Month}-{debito.DataHora.Day}";
            var @events = debito.RecuperarEventos();

            var eventsData = new List<EventData>();
            foreach (var @event in @events)
            {
                var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(@event);
                var eventData = new EventData(Uuid.NewUuid(), @event.GetType().Name, utf8Bytes.AsMemory());
                eventsData.Add(eventData);
            }
            var writeResult = await _eventStore
                .AppendToStreamAsync(streamName,
                                  StreamState.Any,
                                  eventsData);
        }

        public async Task Salvar(Credito credito)
        {
            var streamName = $"FluxoCaixaStream-{credito.DataHora.Year}-{credito.DataHora.Month}-{credito.DataHora.Day}";
            var @events = credito.RecuperarEventos();

            var eventsData = new List<EventData>();
            foreach (var @event in @events)
            {
                var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(@event);
                var eventData = new EventData(Uuid.NewUuid(), @event.GetType().Name, utf8Bytes.AsMemory());
                eventsData.Add(eventData);
            }
            var writeResult = await _eventStore
                .AppendToStreamAsync(streamName,
                                  StreamState.Any,
                                  eventsData);
        }
    }
}
