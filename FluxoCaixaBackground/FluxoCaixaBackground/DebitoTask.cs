using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.DomainServices;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FluxoCaixaBackground
{
    public class DebitoTask : LancamentoTask
    {
        private readonly IDebitoService _debitoService;

        public DebitoTask(ILogger<LancamentoTask> logger, ConnectionFactory factory, IDebitoService debitoService)
            : base(logger, factory)
        {
            _debitoService = debitoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: "debito-lancado-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var debitoMessage = JsonConvert.DeserializeObject<DebitoMessage>(message);

                await _debitoService.Lancar(debitoMessage);
            };

            _channel.BasicConsume(queue: "debito-lancado-queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
