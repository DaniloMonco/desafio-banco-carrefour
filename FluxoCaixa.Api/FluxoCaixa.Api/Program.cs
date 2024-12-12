using FluxoCaixa.Application.Services;
using FluxoCaixa.Domain.Cached;
using FluxoCaixa.Domain.Repository;
using FluxoCaixa.Infrastructure.Cache;
using FluxoCaixa.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IFluxoCaixaService, FluxoCaixaService>();
builder.Services.AddTransient<FluxoCaixa.Domain.DomainService.IFluxoCaixaService, FluxoCaixa.Domain.DomainService.FluxoCaixaService>();

builder.Services.AddTransient<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IDbConnection>(db => new
    Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSql")));

var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis:ConnectionString").Value);
builder.Services.AddSingleton<IConnectionMultiplexer>(connection);

builder.Services.AddSingleton<IFluxoCaixaCached, RedisConnector>();

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
