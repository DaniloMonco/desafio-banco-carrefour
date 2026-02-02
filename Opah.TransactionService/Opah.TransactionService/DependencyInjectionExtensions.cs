using Microsoft.AspNetCore.Authentication;
using Opah.TransactionService.Application.Persistence;
using Opah.TransactionService.Domain.Repositories;
using Opah.TransactionService.Infrastructure.KeyCloak;
using Opah.TransactionService.Infrastructure.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace Opah.TransactionService.Api
{
    public static class DependencyInjectionExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public void AddDependencyInjection()
            {
                builder.Services.AddTransient<IClaimsTransformation, KeycloakRolesTransformation>();

                builder.Services.AddScoped<Application.Services.TransactionService>();

                builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
                builder.Services.AddTransient<ITransactionCreatedOutboxRepository, TransactionCreatedOutboxRepository>();

                builder.Services.AddScoped<DapperContext>();
                builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            }
        }
    }
}
