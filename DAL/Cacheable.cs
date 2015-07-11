using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

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

            CacheItemPolicy policy;
            TimeSpan expiration = new TimeSpan(1, 0, 0);
            var query = String.Format("SELECT c.[Id],c.[ClientUserName] FROM [dbo].[Clients] as c INNER JOIN [dbo].AspNetUsers AS u ON c.UserId = u.Id WHERE u.CoachId = {0}", coachId);
            policy = CacheItemPolicy(query, expiration);
            var coach = Coach.Get(coachId);
            Cache.Set(cacheKey, coach, policy);
            return coach;
        }

        public static void PurgeCoach(int coachId)
        {
            var cacheKey = "{coachId}_Coach".Replace("coachId", coachId.ToString());
            if (Cache.Get(cacheKey) != null) Cache.Remove(cacheKey);
        }

        private static CacheItemPolicy CacheItemPolicy(string query, TimeSpan expiration)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                Priority = CacheItemPriority.NotRemovable,
                SlidingExpiration = expiration,
                RemovedCallback = (args) => Debug.Print("Removed: " + args.CacheItem.Key + " " + args.RemovedReason.ToString())
            };
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DauberDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Connection.Open();
                    var dependency = new SqlDependency(cmd);
                    var monitor = new SqlChangeMonitor(dependency);
                    policy.ChangeMonitors.Add(monitor);
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            return policy;
        }
    }
}
