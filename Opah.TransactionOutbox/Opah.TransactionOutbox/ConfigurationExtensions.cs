using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.TransactionOutbox
{
    public static class ConfigurationExtensions
    {
        extension(HostApplicationBuilder builder)
        {
            public void AddOpenTelemetry()
            {
                var serviceName = builder.Configuration.GetSection("OpenTelemetry:ServiceName").Value!;
                var serviceVersion = builder.Configuration.GetSection("OpenTelemetry:ServiceVersion").Value!;
                var otlpExporter = new Uri(builder.Configuration.GetSection("OpenTelemetry:OtlpExporter").Value!);

                var resourceBuilder = ResourceBuilder.CreateDefault()
                    .AddService(serviceName, serviceVersion);

                builder.Services.AddOpenTelemetry()
                    .WithTracing(t => t.AddSource(serviceName)
                                    .AddHttpClientInstrumentation()
                                    .AddRabbitMQInstrumentation()
                                    .AddNpgsql()
                                    .AddOtlpExporter(o => o.Endpoint = new Uri("http://localhost:4317"))
                                    .SetResourceBuilder(resourceBuilder)
                                    .AddConsoleExporter())
                    .WithMetrics(m => m.AddRuntimeInstrumentation()
                                    .SetResourceBuilder(resourceBuilder)
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
            }
        }
    }
}
