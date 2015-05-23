using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace DAL
{
    public static class Cacheable
    {
        public static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        #region CacheExpirations
        public static CacheItemPolicy Never = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration, Priority = CacheItemPriority.NotRemovable, SlidingExpiration = new TimeSpan(24, 0, 0) };
        public static CacheItemPolicy ShortTerm = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration, Priority = CacheItemPriority.NotRemovable, SlidingExpiration = new TimeSpan(1, 0, 0) };
        public static CacheItemPolicy Shortest = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration, Priority = CacheItemPriority.NotRemovable, SlidingExpiration = new TimeSpan(0, 1, 0) };
        #endregion

        public static bool IsAdmin(string userName)
        {
            if (Cache == null) return Coach.IsAdmin(userName);
            var cacheKey = "{userName}_IsAdmin".Replace("userName", userName);
            if (Cache.Get(cacheKey) != null) return (bool)Cache[cacheKey];
            var isAdmin = Coach.IsAdmin(userName);
            Cache.Set(cacheKey, isAdmin, Shortest);
            return isAdmin;
        }

        public static Coach GetCoach(int coachId)
        {
            if (Cache == null) return Coach.Get(coachId);
            var cacheKey = "{coachId}_Coach".Replace("coachId", coachId.ToString());
            if (Cache.Get(cacheKey) != null) return (Coach)Cache[cacheKey];
            var coach = Coach.Get(coachId);
            Cache.Set(cacheKey, coach, Shortest);
            return coach;
        }

        public static void PurgeCoach(int coachId)
        {
            var cacheKey = "{coachId}_Coach".Replace("coachId", coachId.ToString());
            if (Cache.Get(cacheKey) != null) Cache.Remove(cacheKey);
        }
    }
}
