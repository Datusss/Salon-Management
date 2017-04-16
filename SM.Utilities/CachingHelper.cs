using System.Collections;
using System.Web;

namespace SM.Utilities
{
    public class CachingHelper
    {
        public static bool AddCache(string key, object value)
        {
            try
            {
                if (HttpRuntime.Cache == null) return false;
                var cache = HttpRuntime.Cache;
                //var objcache = cache.Get(key);
                if (cache[key] != null)
                    RemoveCache(key);
                cache.Insert(key, value);
                
                return true;
            }
            catch
            { }
            return false;
        }
        
        public static T GetCache<T>(string key) where T : class
        {
            try
            {
                if (HttpRuntime.Cache == null) return null;
                var cache = HttpRuntime.Cache;
                var obj = (T)cache.Get(key);
                return obj;
            }
            catch
            { }
            return null;
        }
        public static bool RemoveCache(string key)
        {
            try
            {
                if (HttpRuntime.Cache == null) return false;
                var cache = HttpRuntime.Cache;
                cache.Remove(key);
                return true;
            }
            catch
            {
            }
            return false;
        }
        public static bool ResetCache()
        {
            try
            {
                if (HttpRuntime.Cache == null) return false;
                var caches = HttpRuntime.Cache;

                foreach (DictionaryEntry cache in caches)
                {
                    caches.Remove(cache.Key.ToString());
                }
                return true;
            }
            catch
            {}
            
            return false;
        }
    }
}
