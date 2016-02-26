using System;
using System.Text;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.OptionsModel;

namespace MVC6RedisSessionAndCache.Web.Controllers
{
    public class CachingTestController : Controller
    {
        private const string _sessionKey = "SessionKey11";
        private readonly IOptions<RedisCacheOptions> _redisCacheOptions;

        public CachingTestController(IOptions<RedisCacheOptions> redisCacheOptions)
        {
            _redisCacheOptions = redisCacheOptions;
        }

        public IActionResult Index()
        {
            SetCache();

            SetSessionIfNotExists();

            return View();
        }

        private void SetCache()
        {
            var cache = new RedisCache(_redisCacheOptions);

            var key = "CacheKey";
            var value = Encoding.UTF8.GetBytes($"Hello From Redis Caching {DateTime.Now.ToLongTimeString()}");
            cache.Set(key, value, new DistributedCacheEntryOptions());

            var fromCache = Encoding.UTF8.GetString(cache.Get(key));

            ViewData["FromCache"] = fromCache;
        }

        private void SetSessionIfNotExists()
        {
            ViewData["FromSession"] = "Session is not working";
            byte[] sessionOutValue;

            if (HttpContext.Session.TryGetValue(_sessionKey, out sessionOutValue))
            {
                SetViewDataFromSession(sessionOutValue);
                return;
            }

            var sessionValue = Encoding.UTF8.GetBytes($"Hello From Redis Session {DateTime.Now.ToLongTimeString()}");

            HttpContext.Session.Set(_sessionKey, sessionValue);

            if (HttpContext.Session.TryGetValue(_sessionKey, out sessionOutValue))
                SetViewDataFromSession(sessionOutValue);
        }

        private void SetViewDataFromSession(byte[] sessionOutValue)
        {
            var fromSession = Encoding.UTF8.GetString(sessionOutValue);

            ViewData["FromSession"] = fromSession;
        }
    }
}