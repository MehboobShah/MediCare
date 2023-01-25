using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<MediCare.Infrastructure.Caching.LocalCacheService>
{
    protected override MediCare.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<MediCare.Infrastructure.Caching.LocalCacheService>.Instance);
}