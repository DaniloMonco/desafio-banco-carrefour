using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Model;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Infrastructure.EventBus
{
    public class LancamentoEfetuadoEventPublisher : IDebitoLancadoPublisher, ICreditoLancadoPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public LancamentoEfetuadoEventPublisher(ConnectionFactory factory)
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task Publicar(Credito model)
        {
            foreach (var @event in model.RecuperarEventos())
            {
                var json = JsonConvert.SerializeObject(@event);
                var utf8Bytes = Encoding.UTF8.GetBytes(json);
                SendTo("credito.lancado", utf8Bytes);
            }
        }

        public async Task Publicar(Debito model)
        {
            foreach(var @event in model.RecuperarEventos())
            {
                var json = JsonConvert.SerializeObject(@event);
                var utf8Bytes = Encoding.UTF8.GetBytes(json);
                SendTo("debito.lancado", utf8Bytes);
            }
        }

        protected void SendTo(string exchange, byte[] message)
        {
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: "",
                body: message
            );
        }
    }
}
