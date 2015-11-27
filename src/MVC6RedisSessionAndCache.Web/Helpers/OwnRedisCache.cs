using Microsoft.Framework.Caching.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;
using Microsoft.Extensions.DependencyInjection;

namespace MVC6RedisSessionAndCache.Web.Helpers
{
    public class OwnRedisCache : RedisCache, IDistributedCache
    {
        public OwnRedisCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            base.Set(key, value, new Microsoft.Framework.Caching.Distributed.DistributedCacheEntryOptions());
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            return base.SetAsync(key, value, new Microsoft.Framework.Caching.Distributed.DistributedCacheEntryOptions());
        }
    }
}
