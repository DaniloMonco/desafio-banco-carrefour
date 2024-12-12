using FluxoCaixa.Application.Messages;
using FluxoCaixa.Domain.DomainServices;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FluxoCaixaBackground
{
    public class CreditoTask : LancamentoTask
    {
        private readonly ICreditoService _creditoService;

        public CreditoTask(ILogger<LancamentoTask> logger, ConnectionFactory factory, ICreditoService creditoService)
            : base(logger, factory)
        {
            _creditoService = creditoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: "credito-lancado-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var creditoMessage = JsonConvert.DeserializeObject<CreditoMessage>(message);

                await _creditoService.Lancar(creditoMessage);
            };

            _channel.BasicConsume(queue: "credito-lancado-queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
