using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Opah.TransactionOutbox.Application.Publisher;
using Opah.TransactionOutbox.Application.Publisher.Message;
using Opah.TransactionOutbox.Application.Services;
using Opah.TransactionOutbox.Domain.Repositories;
using Opah.TransactionOutbox.Infrastructure.RabbitMQ;
using Opah.TransactionOutbox.Infrastructure.Repositories;
using Opah.TransactionOutbox.Worker;
using Opah.TransactionOutbox;

var builder = Host.CreateApplicationBuilder(args);

builder.AddOpenTelemetry();


builder.Services.AddHostedService<TransactionCreatedTask>();

builder.Services.AddTransient<RabbitMQContext>();
builder.Services.AddTransient<ITransactionEventPublisher<TransactionCreateMessage>, TransactionCreatedPublisher>();

builder.Services.AddTransient<DapperContext>();

builder.Services.AddTransient<ITransactionOutboxRepository, TransactionOutboxRepository>();

builder.Services.AddTransient<TransactionCreatedService>();

var host = builder.Build();
host.Run();
