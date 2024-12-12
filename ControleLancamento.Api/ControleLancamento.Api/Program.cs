using ControleLancamento.Application;
using ControleLancamento.Application.Commands;
using ControleLancamento.Domain.Events;
using ControleLancamento.Domain.Repository;
using ControleLancamento.Infrastructure.EventBus;
using ControleLancamento.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LancarDebitoCommandHandler>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LancarCreditoCommandHandler>());

builder.Services.AddSingleton<IDebitoLancadoPublisher, LancamentoEfetuadoEventPublisher>();
builder.Services.AddSingleton<ICreditoLancadoPublisher, LancamentoEfetuadoEventPublisher>();
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();

builder.Services.AddSingleton(rabbitmq =>
{
    var rabbitMqUrl = builder.Configuration.GetSection("RabbitMq:ConnectionString").Value;
    var factory = new ConnectionFactory() { HostName = rabbitMqUrl };
    factory.DispatchConsumersAsync = true;
    return factory;
});

builder.Services
       .AddEventStoreClient(builder.Configuration
                                    .GetSection("EventStore")
                                    .Get<string>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
