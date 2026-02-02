using Opah.ReportService.Api;
using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Application.Services;

namespace Opah.ReportService.Api
{
    public static class EndpointsExtensions
    {
        extension(WebApplication app)
        {
            public void AddReportEndpoints()
            {
                app.MapGet("/report/{year}/{month}", async (UInt16 year, UInt16 month, IReportService service) =>
                {
                    var @params = new ReportParamsDto(year, month);
                    var report = await service.GetReport(@params);

                    return Results.Ok(report);
                })
                .RequireAuthorization("RequireUserRole")
                .WithName("report");

                app.MapGet("/report/{year}/{month}/{day}", async (UInt16 year, UInt16 month, UInt16 day, IReportService service) =>
                {
                    var @params = new ReportDailyParamsDto(year, month, day);

                    var report = await service.GetReport(@params);

                    return Results.Ok(report);
                })
                .RequireAuthorization("RequireUserRole")
                .WithName("reportByDay");

            }
        }
    }
}
