using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Opah.TransactionService.Api;
using Opah.TransactionService.Application.Dtos;
using Opah.TransactionService.Application.Persistence;
using Opah.TransactionService.Application.Services;
using Opah.TransactionService.Domain.Repositories;
using Opah.TransactionService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.AddAuthentication();
builder.AddAuthorization();

builder.AddOpenTelemetry();


builder.Services.AddEndpointsApiExplorer();
builder.AddSwagger();


builder.AddDependencyInjection();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.UseHttpsRedirection();


app.AddTransactionEndpoints();


app.Run();

