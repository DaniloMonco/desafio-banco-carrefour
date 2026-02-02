using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Application.Cached
{
    public interface IReportCached
    {
        Task<bool> SetAsync<T>(string keyName, T value, TimeSpan? expire);
        Task<T> GetAsync<T>(string keyName);
    }
}
