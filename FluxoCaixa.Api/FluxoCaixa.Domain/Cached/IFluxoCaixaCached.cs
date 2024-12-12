using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Cached
{
    public interface IFluxoCaixaCached
    {
        Task<bool> SetAsync<T>(string redisKeyName, T value, TimeSpan? expire);
        Task<T> GetAsync<T>(string redisKeyName);
    }
}
