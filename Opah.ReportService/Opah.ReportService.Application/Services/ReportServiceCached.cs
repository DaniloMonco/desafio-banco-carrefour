using Opah.ReportService.Application.Cached;
using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Domain.Repositories;

namespace Opah.ReportService.Application.Services
{
    public class ReportServiceCached(IReportRepository repository, IReportCached cache) : ReportService(repository)
    {
        private readonly IReportCached _cache = cache;

        public override async Task<ReportDailyDto> GetReport(ReportDailyParamsDto @params)
        {
            var cachekey = $"GetReport-{@params.Year}-{@params.Month}-{@params.Day}";
            var cacheResult = await _cache.GetAsync<ReportDailyDto>(cachekey);
            if (cacheResult is null)
            {
                var reportDailyDto = await base.GetReport(@params);
                await _cache.SetAsync(cachekey, reportDailyDto, TimeSpan.FromMinutes(5));
                return reportDailyDto;
            }

            return cacheResult;
        }

        public override async Task<ReportDto> GetReport(ReportParamsDto @params)
        {
            var cachekey = $"GetReport-{@params.Year}-{@params.Month}";
            var cachedResult = await _cache.GetAsync<ReportDto>(cachekey);
            if (cachedResult is null)
            {
                var reportDto = await base.GetReport(@params);
                await _cache.SetAsync(cachekey, reportDto, TimeSpan.FromHours(1));
                return reportDto;
            }

            return cachedResult;
        }

    }

   
}
