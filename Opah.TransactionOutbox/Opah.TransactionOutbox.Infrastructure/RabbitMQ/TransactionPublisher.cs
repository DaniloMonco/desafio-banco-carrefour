using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Opah.TransactionOutbox.Infrastructure.RabbitMQ
{
    public abstract class TransactionPublisher<T>(RabbitMQContext context)
    {
        private readonly RabbitMQContext _context = context;

        public async Task Publish(T message)
        {
            var json = JsonConvert.SerializeObject(message);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);

            await using var channel = await _context.CreateChannel();
            await channel.BasicPublishAsync(
                                  exchange: SetExchangeName(),
                                  routingKey: "",
                                  body: utf8Bytes
                                 );
        }

        protected abstract string SetExchangeName();
    }
}
