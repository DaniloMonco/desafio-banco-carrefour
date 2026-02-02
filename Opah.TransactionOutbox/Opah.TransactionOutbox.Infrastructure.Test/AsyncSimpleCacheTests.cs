namespace Opah.TransactionOutbox.Infrastructure.Test
{
    public class AsyncSimpleCacheTests
    {
        [Fact]
        public async Task GetOrAddAsync_Should_Create_Value_Only_Once()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, int>();
            var callCount = 0;

            async Task<int> Factory(string key)
            {
                Interlocked.Increment(ref callCount);
                await Task.Delay(50);
                return 42;
            }

            // Act
            var tasks = Enumerable.Range(0, 10)
                .Select(_ => cache.GetOrAddAsync("key", Factory));

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.All(results, r => Assert.Equal(42, r));
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task Remove_Should_Dispose_Value_When_Completed_Successfully()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, FakeDisposable>();

            var value = await cache.GetOrAddAsync(
                "key",
                _ => Task.FromResult(new FakeDisposable()));

            // Act
            cache.Remove("key");

            // Assert
            Assert.True(value.Disposed);
        }

        [Fact]
        public async Task Remove_Should_Not_Throw_When_Key_Does_Not_Exist()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, object>();

            // Act
            var ex = Record.Exception(() => cache.Remove("invalid-key"));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public async Task GetOrAddAsync_Should_Allow_Recreation_After_Remove()
        {
            // Arrange
            var cache = new AsyncSimpleCache<string, int>();
            var counter = 0;

            async Task<int> Factory(string key)
            {
                counter++;
                return counter;
            }

            // Act
            var first = await cache.GetOrAddAsync("key", Factory);
            cache.Remove("key");
            var second = await cache.GetOrAddAsync("key", Factory);

            // Assert
            Assert.Equal(1, first);
            Assert.Equal(2, second);
        }

        private sealed class FakeDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }
    }
}
