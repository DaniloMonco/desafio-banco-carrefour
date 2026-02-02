using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;
using Opah.ReportService.Api;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

namespace Opah.ReportService.Api
{
    public static class ConfigurationExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public void AddOpenTelemetry(ConnectionMultiplexer redisConnection)
            {
                var serviceName = builder.Configuration.GetSection("OpenTelemetry:ServiceName").Value!;
                var serviceVersion = builder.Configuration.GetSection("OpenTelemetry:ServiceVersion").Value!;
                var otlpExporter = new Uri(builder.Configuration.GetSection("OpenTelemetry:OtlpExporter").Value!);

                var resourceBuilder = ResourceBuilder.CreateDefault()
                    .AddService(serviceName, serviceVersion);

                builder.Services.AddOpenTelemetry()
                    .ConfigureResource(r => r.AddService(serviceName, serviceVersion))
                    .WithTracing(t => t.AddSource(serviceName)
                                    .AddAspNetCoreInstrumentation()
                                    .AddHttpClientInstrumentation()
                                    .AddNpgsql()
                                    .AddRedisInstrumentation(redisConnection)
                                    .AddOtlpExporter(o => o.Endpoint = otlpExporter)
                                    .AddConsoleExporter())
                    .WithMetrics(m => m.AddRuntimeInstrumentation()
                                    .AddAspNetCoreInstrumentation()
                                    .AddOtlpExporter(o => o.Endpoint = otlpExporter)
                                    .AddConsoleExporter())
                    ;

                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Logging.AddOpenTelemetry(l =>
                {
                    l.IncludeScopes = true;
                    l.IncludeFormattedMessage = true;
                    l.ParseStateValues = true;
                    l.AddOtlpExporter(o => o.Endpoint = otlpExporter);
                    l.SetResourceBuilder(resourceBuilder);
                });
            }


            public void AddSwagger()
            {
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Report Api",
                        Version = "v1",
                        Description = ""
                    });

                    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "JWT Authorization header using the Bearer scheme."
                    });

                    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("bearer", document)] = []
                    });
                });
            }

            public void AddAuthentication()
            {
                var authority = builder.Configuration.GetSection("Keycloak:Authority").Value;
                var audience = builder.Configuration.GetSection("Keycloak:Audience").Value;

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                            .AddJwtBearer(options =>
                                            {
                                                options.Authority = authority;
                                                options.Audience = audience;
                                                options.RequireHttpsMetadata = false;
                                                options.TokenValidationParameters = new TokenValidationParameters
                                                {
                                                    ValidateIssuer = true,
                                                    ValidIssuer = authority,
                                                    ValidateAudience = true,
                                                    ValidAudience = audience,
                                                    ValidateLifetime = true,
                                                    ValidateIssuerSigningKey = true,
                                                    NameClaimType = "preferred_username"
                                                };
                                            });
            }

            public void AddAuthorization()
            {
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireUserRole", policy =>
                        policy.RequireRole("report-api-access", "admin"));
                });
            }
        }

        extension(WebApplication app)
        {
            public void UseSwaggerApi()
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Api v1");
                    c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}
