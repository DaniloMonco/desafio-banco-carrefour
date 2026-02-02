using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportInbox.Infrastructure
{
    public sealed class AsyncSimpleCache<TKey, TValue> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _cache = new();

        public Task<TValue> GetOrAddAsync(
            TKey key,
            Func<TKey, Task<TValue>> factory)
        {
            var lazy = _cache.GetOrAdd(
                key,
                k => new Lazy<Task<TValue>>(
                    () => factory(k),
                    LazyThreadSafetyMode.ExecutionAndPublication));

            return lazy.Value;
        }

        public void Remove(TKey key)
        {
            if (_cache.TryRemove(key, out var lazy) &&
                lazy.IsValueCreated &&
                lazy.Value.IsCompletedSuccessfully &&
                lazy.Value.Result is IDisposable d)
            {
                d.Dispose();
            }
        }
    }
}
