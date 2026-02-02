using Newtonsoft.Json;
using Opah.ReportInbox.Application.Consumer.Message;
using Opah.ReportInbox.Application.Idempotency;
using Opah.ReportInbox.Application.Services;
using Opah.ReportInbox.Infrastructure.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Opah.ReportInbox.Worker
{
    public class TransactionCreatedTask(ILogger<TransactionCreatedTask> logger, RabbitMQContext context, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected readonly ILogger<TransactionCreatedTask> _logger = logger;
        protected readonly RabbitMQContext _context = context;
        protected readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected async Task ProcessMessage(BasicDeliverEventArgs args, IChannel channel)
        {
            try
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var transactionMessage = JsonConvert.DeserializeObject<TransactionCreatedMessage>(message)!;

                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<TransactionCreatedService>();
                await service.Process(transactionMessage);
                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch (IdempotencyException ex)
            {
                //Já foi processado, retira da fila e segue
                _logger.LogWarning(ex, "IdempotencyException");
                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wroker error");

                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {


                var channel = await _context.CreateChannel();
                await channel.QueueDeclareAsync(queue: "transaction.queue", durable: true, exclusive: false, autoDelete: false);
                await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (sender, args) =>
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await ProcessMessage(args, channel);
                };

                await channel.BasicConsumeAsync(queue: "transaction.queue",
                                                autoAck: false,
                                                consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker error");
            }
        }
    }
}
