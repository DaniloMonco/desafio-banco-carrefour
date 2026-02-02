using Opah.TransactionOutbox.Application.Services;

namespace Opah.TransactionOutbox.Worker
{
    
    public class TransactionCreatedTask(ILogger<TransactionCreatedTask> logger, TransactionCreatedService service) : BackgroundService
    {
        private readonly ILogger<TransactionCreatedTask> _logger = logger;
        private readonly TransactionCreatedService _service = service;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await _service.Execute();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker error");
                }
            }
        }
    }
}
