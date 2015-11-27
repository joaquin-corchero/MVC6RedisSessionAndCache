using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Caching.Redis;
using Microsoft.Framework.Caching.Distributed;
using System.Text;

namespace MVC6RedisSessionAndCache.Web.Controllers
{
    public class CachingTestController : Controller
    {
        private const string _sessionKey = "SessionKey11";
        private Microsoft.Framework.OptionsModel.IOptions<RedisCacheOptions> _redisCacheOptions;

        public CachingTestController(Microsoft.Framework.OptionsModel.IOptions<RedisCacheOptions> redisCacheOptions)
        {
            this._redisCacheOptions = redisCacheOptions;
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

            string key = "CacheKey";
            byte[] value = Encoding.UTF8.GetBytes($"Hello From Redis Caching {DateTime.Now.ToLongTimeString()}");
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

            byte[] sessionValue = Encoding.UTF8.GetBytes($"Hello From Redis Session {DateTime.Now.ToLongTimeString()}");

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
