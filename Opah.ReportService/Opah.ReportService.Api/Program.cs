using Opah.ReportService.Api;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthentication();
builder.AddAuthorization();


ConfigurationOptions options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
options.AbortOnConnectFail = false;

var redisConnection = ConnectionMultiplexer.Connect(options);


builder.AddOpenTelemetry(redisConnection);

builder.Services.AddEndpointsApiExplorer();
builder.AddSwagger();

builder.AddDependencyInjection(redisConnection);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.AddReportEndpoints();

app.Run();
