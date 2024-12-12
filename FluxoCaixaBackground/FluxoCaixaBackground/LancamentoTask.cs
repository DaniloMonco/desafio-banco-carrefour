using RabbitMQ.Client;

namespace FluxoCaixaBackground
{
    public abstract class LancamentoTask : BackgroundService
    {
        private readonly ILogger<LancamentoTask> _logger;
        protected readonly IModel _channel;
        private readonly IConnection _connection;
        protected LancamentoTask(ILogger<LancamentoTask> logger, ConnectionFactory factory)
        {
            _logger = logger;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
    }
}
