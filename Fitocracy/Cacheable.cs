using System;
using System.Net;
using System.Runtime.Caching;

namespace Fitocracy
{
    public static class Cacheable
    {
        public static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        #region CacheExpirations

        public static CacheItemPolicy Never = new CacheItemPolicy
        {
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
            Priority = CacheItemPriority.NotRemovable,
            SlidingExpiration = new TimeSpan(24, 0, 0)
        };

        public static CacheItemPolicy ShortTerm = new CacheItemPolicy
        {
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
            Priority = CacheItemPriority.NotRemovable,
            SlidingExpiration = new TimeSpan(1, 0, 0)
        };

        public static CacheItemPolicy Shortest = new CacheItemPolicy
        {
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
            Priority = CacheItemPriority.NotRemovable,
            SlidingExpiration = new TimeSpan(0, 1, 0)
        };

        #endregion

        public static CookieCollection Login()
        {
            if (Cache == null) return Scrape.Login();
            const string cacheKey = "FitocracyLoginCookie";
            if (Cache.Get(cacheKey) != null) return (CookieCollection)Cache[cacheKey];
            var loginCookie = Scrape.Login();
            Cache.Set(cacheKey, loginCookie, ShortTerm);
            return loginCookie;
        }
    }
}
