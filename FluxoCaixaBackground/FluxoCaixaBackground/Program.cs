using FluxoCaixa.Domain.DAO;
using FluxoCaixa.Domain.DomainServices;
using FluxoCaixa.Infrastructure.DAO;
using FluxoCaixaBackground;
using RabbitMQ.Client;
using System.Data.Common;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DebitoTask>();
builder.Services.AddHostedService<CreditoTask>();

builder.Services.AddTransient<ILancamentoDao, LancamentoDao>();

builder.Services.AddSingleton(db =>
{
    var factory = DbProviderFactories.GetFactory(new Npgsql.NpgsqlConnection());
    return factory;
});

builder.Services.AddSingleton(rabbitmq =>
{
    var rabbitMqUrl = builder.Configuration.GetSection("RabbitMq:ConnectionString").Value;
    var factory = new ConnectionFactory() { HostName = rabbitMqUrl };
    factory.DispatchConsumersAsync = true;
    return factory;
});

builder.Services.AddTransient<ICreditoService, CreditoService>();
builder.Services.AddTransient<IDebitoService, DebitoService>();
var host = builder.Build();
host.Run();
