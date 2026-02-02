namespace Opah.ReportInbox.Infrastructure.Test
{
    public class AsyncSimpleCacheTests
    {
        [Fact]
        public async Task GetOrAddAsync_Should_Invoke_Factory_Only_Once_For_Same_Key()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, int>();
            var callCount = 0;

            Task<int> Factory(string key)
            {
                Interlocked.Increment(ref callCount);
                return Task.FromResult(42);
            }

            // Act
            var t1 = cache.GetOrAddAsync("key", Factory);
            var t2 = cache.GetOrAddAsync("key", Factory);
            var t3 = cache.GetOrAddAsync("key", Factory);

            var results = await Task.WhenAll(t1, t2, t3);

            // Assert
            Assert.Equal(1, callCount);
            Assert.All(results, r => Assert.Equal(42, r));
        }

        [Fact]
        public async Task GetOrAddAsync_Should_Work_Correctly_With_Concurrent_Calls()
        {
            // Arrange
            var cache = new AsyncSimpleCache<int, Guid>();
            var callCount = 0;

            async Task<Guid> Factory(int key)
            {
                Interlocked.Increment(ref callCount);
                await Task.Delay(50);
                return Guid.NewGuid();
            }

            // Act
            var tasks = Enumerable.Range(0, 10)
                .Select(_ => cache.GetOrAddAsync(1, Factory))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(1, callCount);
            Assert.True(results.All(r => r == results[0]));
        }

        [Fact]
        public async Task Remove_Should_Remove_Item_From_Cache()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, int>();

            await cache.GetOrAddAsync("key", _ => Task.FromResult(100));

            // Act
            cache.Remove("key");

            var result = await cache.GetOrAddAsync("key", _ => Task.FromResult(200));

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public async Task Remove_Should_Dispose_Value_When_It_Is_IDisposable()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, DisposableTestObject>();
            var disposable = new DisposableTestObject();

            await cache.GetOrAddAsync("key", _ => Task.FromResult(disposable));

            // Act
            cache.Remove("key");

            // Assert
            Assert.True(disposable.IsDisposed);
        }

        private sealed class DisposableTestObject : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }
    }
}
