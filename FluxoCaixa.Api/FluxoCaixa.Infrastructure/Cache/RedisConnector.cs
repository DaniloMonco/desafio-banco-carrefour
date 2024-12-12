using FluxoCaixa.Domain.Cached;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Cache
{
    public class RedisConnector : IFluxoCaixaCached
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDatabase _database;

        public RedisConnector(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            _connection = connection;
        }

        protected async Task<object> GetAsync(string redisKeyName)
        {
            var value = await _database.StringGetAsync(new RedisKey(redisKeyName));
            if (value.IsNull)
                return null;
            return value;
        }
        public async Task<T> GetAsync<T>(string redisKeyName)
        {
            var result = await GetAsync(redisKeyName);
            if (result is null)
                return default;

            return JsonConvert.DeserializeObject<T>(result.ToString());
        }

        protected async Task<bool> SetAsync(string redisKeyName, string value, TimeSpan? expire)
            => await _database.StringSetAsync(new RedisKey(redisKeyName), new RedisValue(value), expire);

        public async Task<bool> SetAsync<T>(string redisKeyName, T value, TimeSpan? expire)
            => await SetAsync(redisKeyName, JsonConvert.SerializeObject(value), expire);
    }
}
