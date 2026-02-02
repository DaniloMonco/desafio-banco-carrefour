using Npgsql;
using Opah.ReportInbox.Application.Idempotency;
using Opah.ReportInbox.Application.Services;
using Opah.ReportInbox.Domain.Repositories;
using Opah.ReportInbox.Infrastructure.RabbitMQ;
using Opah.ReportInbox.Infrastructure.Repositories;
using Opah.ReportInbox.Worker;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        const string serviceName = "ReportInbox.Worker";
        const string serviceVersion = "1.0.0";

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName, serviceVersion);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName, serviceVersion))
            .WithTracing(t => t.AddSource(serviceName)
                            .AddHttpClientInstrumentation()
                            .AddRabbitMQInstrumentation()
                            .AddNpgsql()
                            .AddOtlpExporter(o => o.Endpoint = new Uri("http://localhost:4317"))
                            .AddConsoleExporter())
            .WithMetrics(m => m.AddRuntimeInstrumentation()
                            .AddOtlpExporter(o => o.Endpoint = new Uri("http://localhost:4317"))
                            .AddConsoleExporter())
            ;

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddOpenTelemetry(l =>
        {
            l.IncludeScopes = true;
            l.IncludeFormattedMessage = true;
            l.ParseStateValues = true;
            l.AddOtlpExporter(o => o.Endpoint = new Uri("http://localhost:4317"));
            l.SetResourceBuilder(resourceBuilder);
        });

        builder.Services.AddHostedService<TransactionCreatedTask>();

        builder.Services.AddTransient<ITransactionCreatedIdempotency, TransactionCreatedRepository>();
        builder.Services.AddTransient<TransactionCreatedService>();
        builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<DapperContext>();
        builder.Services.AddSingleton<RabbitMQContext>();


        var host = builder.Build();
        host.Run();
    }
}