using Newtonsoft.Json;
using Opah.ReportService.Application.Cached;
using StackExchange.Redis;

namespace Opah.ReportService.Infrastructure.Cached
{
    public class RedisConnector(IConnectionMultiplexer connection) : IReportCached
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<T> GetAsync<T>(string redisKeyName)
        {
            var value = await _database.StringGetAsync(new RedisKey(redisKeyName));
            if (value.IsNull)
                return default!;

            return JsonConvert.DeserializeObject<T>(value.ToString())!;
        }

        protected async Task<bool> SetAsync(string redisKeyName, string value, TimeSpan? expire)
            => await _database.StringSetAsync(new RedisKey(redisKeyName), new RedisValue(value), expire, false);

        public async Task<bool> SetAsync<T>(string redisKeyName, T value, TimeSpan? expire)
            => await SetAsync(redisKeyName, JsonConvert.SerializeObject(value), expire);
    }

}
