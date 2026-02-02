using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Xml.Linq;

namespace Opah.TransactionOutbox.Infrastructure.RabbitMQ
{
    public sealed class RabbitMQContext
    {
        private readonly AsyncSimpleCache<string, IConnection> _connectionCache;

        private ConnectionFactory _connectionFactory { get; }
        public RabbitMQContext(IConfiguration configuration)
        {
            _connectionCache = new AsyncSimpleCache<string, IConnection>();

            var connectionString = configuration.GetConnectionString("RabbitMQ")!;
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };
        }

        private async Task<IConnection> CreateConnection()
        {
            var connectionKey = "TransactionConnectionKey";

            var connection = await _connectionCache.GetOrAddAsync(connectionKey, async _ =>
            {
                var conn = await _connectionFactory.CreateConnectionAsync("TransactionOutbox");

                conn.ConnectionShutdownAsync += async (sender, args)  =>
                {
                    _connectionCache.Remove(connectionKey);
                };

                return conn;
            });

            return connection;
        }

        public async Task<IChannel> CreateChannel()
        {
            var connection = await CreateConnection();
            var channel = await connection.CreateChannelAsync();
            return channel;
        }
    }
}
