using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreV2.Providers.Helper
{
    public class dotNetCore
    {
        /*
         *  Cache 
         *  (Web api)
         * 
         * var cacheKey = "TheTime";
            DateTime existingTime;
            if (_cache.TryGetValue(cacheKey, out existingTime))
            {
                return Ok("Fetched from cache : " + existingTime.ToString());
            }
            else
            {
                existingTime = DateTime.UtcNow;
                _cache.Set(cacheKey, existingTime, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = (DateTime.Now.AddMinutes(1) - DateTime.Now) });
                return Ok("Added to cache : " + existingTime);
            }
         * 
         * 
         * */
    }
}
