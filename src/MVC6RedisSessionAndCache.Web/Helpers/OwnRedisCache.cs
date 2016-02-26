using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.OptionsModel;

namespace MVC6RedisSessionAndCache.Web.Helpers
{
    public class OwnRedisCache : RedisCache, IDistributedCache
    {
        public OwnRedisCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            base.Set(key, value, new DistributedCacheEntryOptions());
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            return base.SetAsync(key, value, new DistributedCacheEntryOptions());
        }
    }
}