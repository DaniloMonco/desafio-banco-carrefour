using Microsoft.AspNetCore.Authentication;
using Opah.ReportService.Api;
using Opah.ReportService.Application.Cached;
using Opah.ReportService.Application.Services;
using Opah.ReportService.Domain.Repositories;
using Opah.ReportService.Infrastructure.Cached;
using Opah.ReportService.Infrastructure.KeyCloak;
using Opah.ReportService.Infrastructure.Repositories;
using StackExchange.Redis;

namespace Opah.ReportService.Api
{
    public static class DependencyInjectionExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public void AddDependencyInjection(IConnectionMultiplexer redisConnection)
            {
                builder.Services.AddTransient<IClaimsTransformation, KeycloakRolesTransformation>();

                builder.Services.AddScoped<IReportService, ReportServiceCached>();

                builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);
                builder.Services.AddSingleton<IReportCached, RedisConnector>();

                builder.Services.AddTransient<IReportRepository, ReportRepository>();
                builder.Services.AddScoped<DapperContext>();
            }
        }
    }
}
